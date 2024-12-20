using BackendApi;
using FrenziedMarmot.DependencyInjection;
using Keycloak.AuthServices.Authentication;
using LoggerLib.Logger;
using MessageLib.Joined;
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

        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

        builder.Logging.ClearProviders();
        builder.Logging.AddColorConsoleLogger();
        builder.Logging.AddFileLogger();

        var assemblies = System.Reflection.Assembly.GetExecutingAssembly().GetReferencedAssemblies();
        var loadedAssemblies = assemblies
            .Select(name => System.Reflection.Assembly.Load(name.ToString()))
            .Append(System.Reflection.Assembly.GetExecutingAssembly())
            .ToArray();

        builder.Services.ScanForAttributeInjection(loadedAssemblies);
        builder.Services.ScanForOptionAttributeInjection(builder.Configuration, loadedAssemblies);


        WebApplication app = builder.Build();
        app.Logger.LogError("test");

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