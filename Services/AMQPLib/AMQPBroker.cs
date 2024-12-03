namespace AMQPLib
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using RabbitMQ.Client;
    using System.Threading.Tasks;

    public class AMQPBroker : IDisposable
    {
        private readonly ILogger<AMQPBroker>? logger;

        private readonly ConnectionFactory factory;

        private IConnection? connection;
        private IChannel? channel;
        private readonly Func<Task> connectionTask;

        private readonly HashSet<string> managedQueues = [];

        private bool disposedValue;

        // TODO handle errors from rabbit mq or network
        public AMQPBroker(ILogger<AMQPBroker>? logger, IOptions<RabbitMQOptions>? options)
        {
            ArgumentNullException.ThrowIfNull(options);

            this.logger=logger;
            this.factory=new ConnectionFactory
            {
                HostName=options.Value.Host,
                UserName=options.Value.Username,
                Password=options.Value.Password,
                Port=options.Value.Port,
                VirtualHost=options.Value.VirtualHost,
            };

            this.connectionTask=async () =>
            {
                if (this.connection==null||this.channel==null)
                {
                    await Task.Run(async () =>
                    {
                        this.connection=await this.factory.CreateConnectionAsync();
                        this.channel=await this.connection.CreateChannelAsync();

                        this.channel.CallbackExceptionAsync+=this.Channel_CallbackExceptionAsync;
                    });
                }
            };
        }

        private Task Channel_CallbackExceptionAsync(object sender, RabbitMQ.Client.Events.CallbackExceptionEventArgs @event)
        {
            return Task.Run(() =>
            {
                this.logger?.LogDebug(@event.Exception.Message);
            });
        }

        public async Task CreateQueueAsync(string Name)
        {
            await this.connectionTask();

            lock (this.managedQueues)
            {
                if (this.managedQueues.Contains(Name))
                {
                    this.logger?.LogError("A queue with the name {0} already exists", Name);
                    throw new ArgumentException($"A queue with the name {Name} already exists");
                }

                this.managedQueues.Add(Name);
            }

            await this.channel!.QueueDeclareAsync(queue: "frontend."+Name, durable: false, exclusive: false, autoDelete: false, arguments: null);
            await this.channel.QueueDeclareAsync(queue: "backend."+Name, durable: false, exclusive: false, autoDelete: false, arguments: null);

            await this.channel.QueueBindAsync(queue: "backend."+Name, exchange: "backendEx", routingKey: Name);
            await this.channel.QueueBindAsync(queue: "frontend."+Name, exchange: "frontendEx", routingKey: Name);
        }

        public async Task RemoveQueueAsync(string Name)
        {
            await this.connectionTask();

            lock (this.managedQueues)
            {
                if (!this.managedQueues.Contains(Name))
                {
                    return;
                }

                this.managedQueues.Remove(Name);
            }

            await this.channel!.QueueDeleteAsync(queue: "frontend."+Name, false, false);
            await this.channel.QueueDeleteAsync(queue: "backend."+Name, false, false);
        }

        public async Task SendMessageAsync(string Queue, ReadOnlyMemory<byte> bytes, uint ttl)
        {
            if (!this.managedQueues.Contains(Queue))
            {
                throw new InvalidOperationException();
            }

            await this.connectionTask();
            await Task.Run(async () =>
            {
                BasicProperties properties = new BasicProperties
                {
                    Expiration=ttl.ToString()
                };

                await this.channel!.BasicPublishAsync("backendEx", Queue, true, properties, bytes);
            });

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null

                this.channel?.Dispose();
                this.connection?.Dispose();
                this.disposedValue=true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~AMQPBroker()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
