namespace BackendApi
{
    using FrenziedMarmot.DependencyInjection;

    [Injectable(Lifetime = ServiceLifetime.Singleton)]
    public class KnownClientStore : Dictionary<string, KnownClientInfo>
    {
        // TODO make thread safe if needed
    }

    public class KnownClientInfo
    {
        public required string lobbyId { get; init; }

        public required CancellationTokenSource token { get; init; }
    }
}
