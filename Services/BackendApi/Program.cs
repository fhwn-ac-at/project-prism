using AMQPLib;
using Keycloak.AuthServices.Authentication;
using LoggerLib;
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

        builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
        builder.Services.AddSwaggerGen();

        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

        builder.Logging.ClearProviders();
        builder.Logging.AddColorConsoleLogger();
        builder.Logging.AddFileLogger();

        builder.Services.Configure<FileOpenerOptions>(builder.Configuration.GetSection("FileOptions"));
        builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection("AMQPOptions"));
        builder.Services.AddSingleton<FileOpener, FileOpener>();
        builder.Services.AddSingleton<AMQPBroker, AMQPBroker>();

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

        app.Run();
    }
}