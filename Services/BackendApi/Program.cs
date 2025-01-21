using BackendApi.ApiClients;
using BackendApi.MessageDistributing;
using FrenziedMarmot.DependencyInjection;
using Keycloak.AuthServices.Authentication;
using LoggerLib.Logger;
using MessageLib.Joined;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        //builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSignalR();

        builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
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
        builder.Services.AddHttpClient();
        builder.Services.AddTransient<GeneratedGameClientFactory>();

        // build cors
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200/").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
        });

        WebApplication app = builder.Build();

        // use cors
        app.UseCors();

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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHub<AMQPBridgeHub>("ws/{client-id}");

        app.Run();
    }
}