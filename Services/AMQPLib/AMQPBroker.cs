namespace AMQPLib
{
    using FrenziedMarmot.DependencyInjection;
    using MessageLib;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System.Threading.Tasks;


    [Injectable(Lifetime = ServiceLifetime.Singleton, TargetType = typeof(IAMQPQueueManager))]
    [Injectable(Lifetime = ServiceLifetime.Singleton, TargetType = typeof(AMQPBroker))]
    public class AMQPBroker : IDisposable, IAMQPQueueManager
    {
        private readonly ILogger<AMQPBroker>? logger;
        private readonly IServiceProvider serviceProvider;

        private readonly ConnectionFactory factory;

        private IConnection? connection;
        private IChannel? channel;
        private readonly Func<Task> connectionTask;

        private readonly HashSet<string> managedQueues = [];
        private readonly Dictionary<string, string> consumerTags = [];

        private bool disposedValue;

        public event EventHandler<string>? receivedMessage;

        // TODO handle errors from rabbit mq or network
        public AMQPBroker(ILogger<AMQPBroker>? logger, IOptions<RabbitMQOptions>? options, IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(options);

            if (options.Value.Username == null)
            {
                throw new ArgumentNullException(nameof(options.Value.Username));
            }

            if (options.Value.Password==null)
            {
                throw new ArgumentNullException(nameof(options.Value.Password));
            }

            if (options.Value.VirtualHost==null)
            {
                throw new ArgumentNullException(nameof(options.Value.VirtualHost));
            }

            this.logger=logger;
            this.serviceProvider=serviceProvider;
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

        // TODO error handling remove consumer tag on error
        public async Task CreateQueueAsync(string name)
        {
            lock (this.managedQueues)
            {
                if (this.managedQueues.Contains(name))
                {
                    this.logger?.LogError("A queue with the name {0} already exists", name);
                    throw new ArgumentException($"A queue with the name {name} already exists");
                }

                this.managedQueues.Add(name);
            }

            await this.connectionTask();

            await this.channel!.QueueDeclareAsync(queue: "game."+name, durable: false, exclusive: false, autoDelete: false, arguments: null);
            await this.channel.QueueDeclareAsync(queue: "backend."+name, durable: false, exclusive: false, autoDelete: false, arguments: null);

            await this.channel.QueueBindAsync(queue: "backend."+name, exchange: "backendEx", routingKey: name);
            await this.channel.QueueBindAsync(queue: "game."+name, exchange: "gameEx", routingKey: name);
        }

        public async Task RemoveQueueAsync(string Name)
        {
            lock (this.managedQueues)
            {
                if (!this.managedQueues.Contains(Name))
                {
                    return;
                }

                this.managedQueues.Remove(Name);
            }

            await this.connectionTask();

            var gameDeletionTask = this.channel!.QueueDeleteAsync(queue: "game."+Name, false, false);
            var backendDeletionTask = this.channel.QueueDeleteAsync(queue: "backend."+Name, false, false);

            string? consumerTag;
            lock (this.consumerTags)
            {
                if (!this.consumerTags.TryGetValue(Name, out consumerTag))
                {
                    return;
                }
            }

            await this.channel!.BasicCancelAsync(consumerTag);
            await gameDeletionTask;
            await backendDeletionTask;
        }

        public async Task ConnectToQueueAsync(string name, string prefix, IMessageDistributor messageDistributor)
        {
            lock (this.managedQueues)
            {
                if (!this.managedQueues.Contains(name))
                {
                    try
                    {
                        // check if queue exists
                        this.channel?.QueueDeclarePassiveAsync(name);
                        this.managedQueues.Add(name);
                    } catch (RabbitMQ.Client.Exceptions.OperationInterruptedException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
            await this.connectionTask();

            var consumer = new AsyncEventingBasicConsumer(this.channel!);
            var distributor = new AMQPMessageDistributor(messageDistributor, this.serviceProvider.GetRequiredService<ILogger<AMQPMessageDistributor>>());
            consumer.ReceivedAsync+=async (object sender, BasicDeliverEventArgs args) =>
            {
                await distributor.HandleReceivedAsync(sender, args);
                await channel!.BasicAckAsync(deliveryTag: args.DeliveryTag, multiple: false);
            };
            // this consumer tag identifies the subscription
            // when it has to be cancelled
            string consumerTag = await this.channel!.BasicConsumeAsync(prefix+"."+name, false, consumer);

            lock (this.consumerTags)
            {
                this.consumerTags.Add(name, consumerTag);
            }
        }

        public async Task SendMessageAsync(string exchange, string queue, ReadOnlyMemory<byte> bytes, uint ttl)
        {
            lock (this.managedQueues)
            {
                if (!this.managedQueues.Contains(queue))
                {
                    throw new InvalidOperationException();
                }
            }

            await this.connectionTask();
            await Task.Run(async () =>
            {
                BasicProperties properties = new BasicProperties
                {
                    Expiration=ttl.ToString()
                };

                await this.channel!.BasicPublishAsync(exchange, queue, true, properties, bytes);
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
