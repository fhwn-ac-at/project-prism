namespace BackendApi.Controllers
{
    using AMQPLib;
    using Keycloak.AuthServices.Sdk;
    using Keycloak.AuthServices.Sdk.Admin;
    using Microsoft.AspNetCore.Authorization;
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
            this.logger=logger;
            this.broker=broker;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            this.logger.LogInformation(this.User.ExtractDisplayName());

            ServiceCollection services = new ServiceCollection();
            services.AddKeycloakAdminHttpClient(new KeycloakAdminClientOptions
            {
                AuthServerUrl="http://localhost:8180/",
                Realm="prism",
                Resource="admin-cli",
            });

            ServiceProvider sp = services.BuildServiceProvider();
            IKeycloakRealmClient client = sp.GetRequiredService<IKeycloakRealmClient>();

            Keycloak.AuthServices.Sdk.Admin.Models.RealmRepresentation realm = await client.GetRealmAsync("prism");

            return this.User.ExtractDisplayName();
        }
    }
}
