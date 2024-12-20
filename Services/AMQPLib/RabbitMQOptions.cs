namespace AMQPLib
{
    using FrenziedMarmot.DependencyInjection;

    [InjectableOptions("AMQPOptions")]
    public record RabbitMQOptions
    {
        public string Host { get; init; } = "localhost";

        public ushort Port { get; init; } = 5672;

        public string? Username { get; init; }

        public string? Password { get; init; }

        public string? VirtualHost { get; init; }
    }
}
