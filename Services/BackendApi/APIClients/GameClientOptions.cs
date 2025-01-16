using FrenziedMarmot.DependencyInjection;

[InjectableOptions("GameServiceOptions")]
public class GameClientOptions
{
    public string BaseUrl { get; init; }
}