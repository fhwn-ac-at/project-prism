namespace BackendApi
{
    using BackendApi.MessageDistributing;
    using FrenziedMarmot.DependencyInjection;
    using System.Collections.Concurrent;

    [Injectable(Lifetime = ServiceLifetime.Singleton)]
    public class KnownConnectionsStore : ConcurrentDictionary<string, WebsocketToAMQPConnector>
    {
        public Guid ConnectionId { get; } = Guid.NewGuid();
    }
}
