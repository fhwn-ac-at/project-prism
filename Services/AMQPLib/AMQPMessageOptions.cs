namespace AMQPLib
{
    using FrenziedMarmot.DependencyInjection;

    [InjectableOptions("AMQPMessageOptions")]
    public class AMQPMessageOptions
    {
        public required uint TimeToLive { get; init; }
    }
}
