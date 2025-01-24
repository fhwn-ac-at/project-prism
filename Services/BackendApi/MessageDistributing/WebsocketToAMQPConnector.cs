namespace BackendApi.MessageDistributing
{
    using AMQPLib;
    using MessageLib;
    using Microsoft.AspNetCore.SignalR;

    public class WebsocketToAMQPConnector
    {
        private readonly ISingleClientProxy clientProxy;
        private readonly IAMQPBroker broker;
        private readonly string userId;
        private readonly Validator validator;
        private readonly ILogger<WebsocketToAMQPConnector>? logger;

        public WebsocketToAMQPConnector(ISingleClientProxy clientProxy, string userId, IAMQPBroker broker, PlainMessageDistributor messageDistributor, Validator validator, ILogger<WebsocketToAMQPConnector>? logger = null)
        {
            this.clientProxy=clientProxy;
            this.broker=broker;
            this.userId=userId;
            this.validator=validator;
            this.logger=logger;
            this.broker.ConnectToQueueAsync(userId, messageDistributor);
            messageDistributor.ValidMessageReceived+=this.ValidMessageReceived;
        }

        public async Task SendAsync(string message)
        {
            if (!this.validator.Validate(message, out _))
            {
                this.logger?.LogInformation("Message not valid! Message: {}", message);
                return;
            }

            await this.broker.SendMessageAsync(this.userId, System.Text.Encoding.UTF8.GetBytes(message), Convert.ToUInt32(TimeSpan.FromMinutes(10).TotalMilliseconds));
        }


        private void ValidMessageReceived(object? sender, string message)
        {
            Task.Run(async () =>
            {
                await this.clientProxy.SendAsync("Frontend", message);
                this.logger?.LogTrace("Send message to Frontend: {}", message);
            });
        }
    }
}