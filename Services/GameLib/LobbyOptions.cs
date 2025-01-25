namespace GameLib
{
    using FrenziedMarmot.DependencyInjection;

    [InjectableOptions("LobbyOptions")]
    public class LobbyOptions
    {
        public required ushort DefaultRoundAmount { get; init; }
        public required ushort DefaultRoundDuration { get; init; }
    }
}
