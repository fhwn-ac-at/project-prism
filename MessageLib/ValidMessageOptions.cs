namespace MessageLib
{
    using FrenziedMarmot.DependencyInjection;

    [InjectableOptions("ValidMessageOptions")]
    public class ValidMessageOptions
    {
        public required double MaxMessageDifference { get; init; }
    }
}
