namespace BackendApi
{
    using FrenziedMarmot.DependencyInjection;

    [Injectable(Lifetime = ServiceLifetime.Singleton)]
    public class KnownClientStore : Dictionary<string, string>
    {
    }
}
