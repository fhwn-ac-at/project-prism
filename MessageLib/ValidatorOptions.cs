namespace MessageLib
{
    using FrenziedMarmot.DependencyInjection;

    [InjectableOptions("ValidatorOptions")]
    public class ValidatorOptions
    {
        public required bool useNewtonsoftJsonSchemaValidator { get; init; }
    }
}
