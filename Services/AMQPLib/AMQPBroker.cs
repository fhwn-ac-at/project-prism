namespace AMQPLib
{
    using FrenziedMarmot.DependencyInjection;
    using MessageLib;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using RabbitMQ.Client.Exceptions;
    using System;
    using System.Threading.Tasks;


    [Injectable(Lifetime = ServiceLifetime.Singleton, TargetType = typeof(IAMQPQueueManager))]
    [Injectable(Lifetime = ServiceLifetime.Singleton, TargetType = typeof(AMQPBroker))]
    public class AMQPBroker : IDisposable, IAMQPQueueManager
    {
        private readonly ILogger<AMQPBroker>? logger;
        private readonly IServiceProvider serviceProvider;
        private readonly uint ttl;

        private readonly ConnectionFactory factory;

        private IConnection? connection;
        private IChannel? channel;
        private readonly Func<Task> connectionTask;

        private readonly HashSet<string> managedQueues = [];
        private readonly Dictionary<string, string> consumerTags = [];

        private bool disposedValue;

        public event EventHandler<string>? receivedMessage;

        // TODO handle errors from rabbit mq or network
        public AMQPBroker(ILogger<AMQPBroker>? logger, IOptions<RabbitMQOptions>? options, IOptions<AMQPMessageOptions> messageOptions, IServiceProvider serviceProvider)
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
            this.ttl=messageOptions.Value.TimeToLive;
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
                if (this.connection==null||this.channel==null || !this.connection.IsOpen || !this.channel.IsOpen)
                {
                    if (this.logger != null)
                    {
                        string reason = "";

                        if (this.connection == null)
                        {
                            reason+="connection null ";
                        }
                        if (this.channel == null)
                        {
                            reason+="channel null ";
                        }
                        if (this.connection != null && !this.connection.IsOpen)
                        {
                            reason+="connection not open ";
                        }
                        if (this.channel != null && this.channel.IsClosed)
                        {
                            reason+="channel not open ";
                        }

                        this.logger?.LogTrace("Reconnecting object {} because of {}", this.GetHashCode(), reason);
                    }

                    if (this.connection == null ||!this.connection.IsOpen)
                    {
                        this.connection=await this.factory.CreateConnectionAsync();
                    }

                    if (this.channel==null ||this.channel.IsClosed)
                    {
                        this.channel=await this.connection.CreateChannelAsync();
                        this.channel.CallbackExceptionAsync+=this.Channel_CallbackExceptionAsync;
                    }
                }
            };
        }

        private Task Channel_CallbackExceptionAsync(object sender, RabbitMQ.Client.Events.CallbackExceptionEventArgs @event)
        {
            return Task.Run(() =>
            {
                this.logger?.LogError(@event.Exception.Message);
            });
        }

        // TODO error handling remove consumer tag on error
        public async Task CreateQueueAsync(string name)
        {
            lock (this.managedQueues)
            {
                if (this.managedQueues.Contains(name))
                {
                    this.logger?.LogError("A queue with the name {} already exists", name);
                    throw new ArgumentException($"A queue with the name {name} already exists");
                }

                this.managedQueues.Add(name);
            }

            await this.connectionTask();

            await this.channel!.QueueDeclareAsync(queue: "game."+name, durable: false, exclusive: false, autoDelete: false, arguments: null);
            await this.channel.QueueDeclareAsync(queue: "backend."+name, durable: false, exclusive: false, autoDelete: false, arguments: null);

            await this.channel.QueueBindAsync(queue: "backend."+name, exchange: "backendEx", routingKey: name);
            await this.channel.QueueBindAsync(queue: "game."+name, exchange: "gameEx", routingKey: name);

            this.logger?.LogDebug("Created queue with the name {}", name);
        }

        public async Task RemoveQueueAsync(string name)
        {
            lock (this.managedQueues)
            {
                if (!this.managedQueues.Contains(name))
                {
                    this.logger?.LogWarning("Deleted queue doesn't exist {}", name);
                    return;
                }

                this.managedQueues.Remove(name);
            }

            await this.connectionTask();

            var gameDeletionTask = this.channel!.QueueDeleteAsync(queue: "game."+name, false, false);
            var backendDeletionTask = this.channel.QueueDeleteAsync(queue: "backend."+name, false, false);

            string? consumerTag;
            Task? cancelTask = null;
            lock (this.consumerTags)
            {
                if (this.consumerTags.TryGetValue(name, out consumerTag))
                {
                    cancelTask= this.channel!.BasicCancelAsync(consumerTag);
                }
            }
            
            if (cancelTask != null)
            {
                await cancelTask;
            }
            
            await gameDeletionTask;
            await backendDeletionTask;


            this.logger?.LogDebug("Deleted queue with the name {}", name);
        }

        public async Task ConnectToQueueAsync(string name, string prefix, IMessageDistributor messageDistributor)
        {
            this.logger?.LogTrace("Connect to {}.{}", prefix, name);
            var wasCreatedByObject = false;
            await this.connectionTask();
            lock (this.managedQueues)
            {
                if (this.managedQueues.Contains(name))
                {
                    wasCreatedByObject=true;
                }
            }

            if (!wasCreatedByObject)
            {
                try
                {
                    // check if queue exists
                    await this.channel!.QueueDeclarePassiveAsync($"{prefix}.{name}");
                    lock (this.managedQueues)
                    {
                        this.managedQueues.Add(name);
                    }
                }
                catch (RabbitMQ.Client.Exceptions.OperationInterruptedException)
                {
                    this.logger?.LogError("Queue with name {}.{} doesn't exist", prefix, name);
                    throw new InvalidOperationException();
                }
            }

            var consumer = new AsyncEventingBasicConsumer(this.channel!);
            var distributor = new AMQPMessageDistributor(messageDistributor, this.serviceProvider.GetRequiredService<ILogger<AMQPMessageDistributor>>());
            consumer.ReceivedAsync+=async (object sender, BasicDeliverEventArgs args) =>
            {
                this.logger?.LogTrace("Got Message mq");
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

        public async Task SendMessageAsync(string exchange, string queue, string queuePrefix, ReadOnlyMemory<byte> bytes)
        {
            this.logger?.LogTrace("Sending to exchange {} for queue {}", exchange, queue);
            var wasCreatedByObject = false;
            await this.connectionTask();
            lock (this.managedQueues)
            {
                if (this.managedQueues.Contains(queue))
                {
                    wasCreatedByObject=true;
                }
            }

            if (!wasCreatedByObject)
            {
                try
                {
                    // check if queue exists
                    // exchange name wizthout Ex . queueu
                    await this.channel!.QueueDeclarePassiveAsync($"{queuePrefix}.{queue}");
                    lock (this.managedQueues)
                    {
                        this.managedQueues.Add(queue);
                    }
                }
                catch (RabbitMQ.Client.Exceptions.OperationInterruptedException)
                {
                    this.logger?.LogError("Queue with key {}.{} doesn't exist", queuePrefix, queue);
                    throw new InvalidOperationException();
                }
            }

            await this.connectionTask();
            await Task.Run(async () =>
            {
                BasicProperties properties = new BasicProperties
                {
                    Expiration=this.ttl.ToString()
                };

                try
                {
                    await this.channel!.BasicPublishAsync(exchange, queue, false, properties, bytes);
                    this.logger?.LogTrace("Published to exchange {} for queue {}", exchange, queue);
                }
                catch (PublishException e)
                {
                    this.logger?.LogError("Message could not be published for exchange {} for queue {} mit errorMessage {}", exchange, queue, e.Message);
                }
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
