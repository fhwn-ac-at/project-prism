namespace BackendApi.Controllers
{
    using BackendApi.AMQP;
    using Keycloak.AuthServices.Sdk;
    using Keycloak.AuthServices.Sdk.Admin;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QueueCreationController : ControllerBase
    {
        private readonly ILogger<QueueCreationController> logger;
        private readonly AMQPBroker broker;

        public QueueCreationController(ILogger<QueueCreationController> logger, AMQPBroker broker)
        {
            this.logger = logger;
            this.broker = broker;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            this.logger.LogInformation(this.User.ExtractDisplayName());

            var services = new ServiceCollection();
            services.AddKeycloakAdminHttpClient(new KeycloakAdminClientOptions
            {
                AuthServerUrl="http://localhost:8180/",
                Realm="prism",
                Resource="admin-cli",
            });

            var sp = services.BuildServiceProvider();
            var client = sp.GetRequiredService<IKeycloakRealmClient>();

            var realm = await client.GetRealmAsync("prism");

            return this.User.ExtractDisplayName();
        }
    }
}
