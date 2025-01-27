namespace BackendApi.MessageDistributing
{
    using FrenziedMarmot.DependencyInjection;
    using MessageLib;
    using Microsoft.Extensions.Options;

    [Injectable(Lifetime = ServiceLifetime.Transient, TargetType = typeof(IMessageDistributor))]
    [Injectable(Lifetime = ServiceLifetime.Transient, TargetType = typeof(PlainMessageDistributor))]
    public class PlainMessageDistributor(Validator validator, IOptions<ValidMessageOptions> messageOptions, ILogger<PlainMessageDistributor>? logger = null) : IMessageDistributor
    {
        public event EventHandler<string>? ValidMessageReceived;
        private readonly Validator validator = validator;
        private readonly ILogger<PlainMessageDistributor>? logger = logger;
        private readonly double maxMessageTimeout = messageOptions.Value.MaxMessageDifference;

        public bool HandleMessage(string message)
        {
            this.logger?.LogTrace("Got Message: {}", message);
            if (!this.validator.ValidateWithCheckTimestamp(message, out _, this.maxMessageTimeout))
            {
                this.logger?.LogInformation("Message not valid! Message: {}", message);
                return false;
            }

            this.ValidMessageReceived?.Invoke(this, message);

            return true;
        }
    }
}