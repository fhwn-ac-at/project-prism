using Keycloak.AuthServices.Authentication;
using System.Security.Claims;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

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

        app.MapGet("/", (ClaimsPrincipal user) =>
        {
            app.Logger.LogInformation(user.Identity.Name);
            app.Logger.LogInformation(user.Identity.AuthenticationType);
        }).RequireAuthorization();

        app.Run();
    }
}