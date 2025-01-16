namespace AMQPLib
{
    using FrenziedMarmot.DependencyInjection;
    using MessageLib;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using RabbitMQ.Client.Events;

    [Injectable(Lifetime = ServiceLifetime.Transient)]
    public class AMQPMessageDistributor(IMessageDistributor distributor, ILogger<AMQPMessageDistributor>? logger = null)
    {
        private readonly ILogger<AMQPMessageDistributor>? logger = logger;
        private readonly IMessageDistributor distributor = distributor;

        public IMessageDistributor Distributor { get { return distributor; } }

        public async Task HandleReceivedAsync(object Sender, BasicDeliverEventArgs args)
        {
            // TODO error handling
            byte[] bodyBytes = args.Body.ToArray();
            string message = System.Text.Encoding.UTF8.GetString(bodyBytes);
            await Task.Run(() =>
            {
                _ = this.distributor.HandleMessage(message);
            });
        }
    }
}