namespace BackendApi
{
    using FrenziedMarmot.DependencyInjection;

    [InjectableOptions("ConnectionConfirmOptions")]
    public class ConnectionConfirmOptions
    {
        public required ushort ConfirmationThreshold { get; init; }
    }
}
