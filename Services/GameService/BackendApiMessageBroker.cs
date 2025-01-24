namespace BackendApi.MessageDistributing
{
    using AMQPLib;
    using FrenziedMarmot.DependencyInjection;
    using MessageLib;
    using System;
    using System.Threading.Tasks;

    [Injectable(Lifetime = ServiceLifetime.Transient, TargetType = typeof(IAMQPBroker))]
    public class GameApiMessageBroker : IAMQPBroker
    {
        private readonly AMQPBroker broker;
        private readonly ILogger<GameApiMessageBroker>? logger;

        public GameApiMessageBroker(AMQPBroker broker, ILogger<GameApiMessageBroker>? logger = null)
        {
            this.broker=broker;
            this.logger=logger;
        }

        public Task ConnectToQueueAsync(string name, IMessageDistributor messageDistributor)
        {
            // TODO povide exchanges via enum from AMQP service
            return this.broker.ConnectToQueueAsync(name, "backend", messageDistributor);
        }

        public Task SendMessageAsync(string queue, ReadOnlyMemory<byte> bytes, uint ttl)
        {
            this.logger?.LogTrace("Sent to queue: {} with ttl: {}", queue, ttl);
            return this.broker.SendMessageAsync("gameEx", queue, bytes, ttl);
        }
    }
}
