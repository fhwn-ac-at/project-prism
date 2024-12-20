namespace BackendApi
{
    using FrenziedMarmot.DependencyInjection;
    using MessageLib;

    [Injectable(Lifetime = ServiceLifetime.Transient, TargetType = typeof(IMessageDistributor))]
    public class PlainMessageDistributor(Validator validator, ILogger<PlainMessageDistributor>? logger = null) : IMessageDistributor
    {
        public event EventHandler<string>? ValidMessageReceived;
        private readonly Validator validator = validator;
        private readonly ILogger<PlainMessageDistributor>? logger = logger;

        public bool HandleMessage(string message)
        {
            if (!this.validator.Validate(message, out _))
            {
                this.logger?.LogInformation("Message not valid! Message: {}", message);
                return false;
            }

            this.ValidMessageReceived?.Invoke(this, message);

            return true;
        }
    }
}