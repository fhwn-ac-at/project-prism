namespace BackendApi.MessageDistributing
{
    using AMQPLib;
    using FrenziedMarmot.DependencyInjection;
    using MessageLib;
    using System;
    using System.Threading.Tasks;

    [Injectable(Lifetime = ServiceLifetime.Transient, TargetType = typeof(IAMQPBroker))]
    public class BackendApiMessageBroker : IAMQPBroker
    {
        private readonly AMQPBroker broker;
        private readonly ILogger<BackendApiMessageBroker>? logger;

        public BackendApiMessageBroker(AMQPBroker broker, ILogger<BackendApiMessageBroker>? logger = null)
        {
            this.broker=broker;
            this.logger=logger;
        }

        public Task ConnectToQueueAsync(string name, IMessageDistributor messageDistributor)
        {
            // TODO povide exchanges via enum from AMQP service
            return this.broker.ConnectToQueueAsync(name, "game", messageDistributor);
        }

        public Task SendMessageAsync(string queue, ReadOnlyMemory<byte> bytes, uint ttl)
        {
            return this.broker.SendMessageAsync("backendEx", queue, bytes, ttl);
        }
    }
}
