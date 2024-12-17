namespace AMQPLib
{
    using MessageLib;
    using Microsoft.Extensions.Logging;
    using RabbitMQ.Client.Events;

    public class AMQPMessageDistributor(MessageDistributor distributor, ILogger<AMQPMessageDistributor>? logger = null)
    {
        private readonly ILogger<AMQPMessageDistributor>? logger = logger;
        private readonly MessageDistributor distributor = distributor;

        public MessageDistributor Distributor { get { return distributor; } }

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