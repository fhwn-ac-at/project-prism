namespace LoggerLib
{
    using FrenziedMarmot.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;

    [Injectable(Lifetime = ServiceLifetime.Singleton)]
    public class LoggerLockObject
    {
    }
}
