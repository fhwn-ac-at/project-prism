namespace BackendApi
{
    using BackendApi.MessageDistributing;
    using FrenziedMarmot.DependencyInjection;

    [Injectable(Lifetime = ServiceLifetime.Singleton)]
    public class KnownConnectionsStore : Dictionary<string, WebsocketToAMQPConnector>
    {
    }
}
