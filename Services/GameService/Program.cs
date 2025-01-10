using FrenziedMarmot.DependencyInjection;
using LoggerLib.Logger;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Logging.ClearProviders();
        builder.Logging.AddColorConsoleLogger();
        builder.Logging.AddFileLogger();
        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

        var assemblies = System.Reflection.Assembly.GetExecutingAssembly().GetReferencedAssemblies();
        var loadedAssemblies = assemblies
            .Select(name => System.Reflection.Assembly.Load(name.ToString()))
            .Append(System.Reflection.Assembly.GetExecutingAssembly())
            .ToArray();

        builder.Services.ScanForAttributeInjection(loadedAssemblies);
        builder.Services.ScanForOptionAttributeInjection(builder.Configuration, loadedAssemblies);

        WebApplication app = builder.Build();
        app.Logger.LogCritical("critical");
        app.Logger.LogError("error");
        app.Logger.LogWarning("warning");
        app.Logger.LogInformation("information");
        app.Logger.LogDebug("debug");
        app.Logger.LogTrace("trace");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}