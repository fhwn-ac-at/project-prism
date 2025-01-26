namespace BackendApi
{
    using FrenziedMarmot.DependencyInjection;
    using System.Collections.Concurrent;

    [Injectable(Lifetime = ServiceLifetime.Singleton)]
    public class KnownClientStore : ConcurrentDictionary<string, KnownClientInfo>
    {
    }

    public class KnownClientInfo
    {
        public required string lobbyId { get; init; }

        public required CancellationTokenSource token { get; init; }
    }
}
