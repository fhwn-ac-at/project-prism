namespace BackendApi.Controllers
{
    using AMQPLib;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LobbyController : ControllerBase
    {
        private readonly ILogger<LobbyController> logger;
        private readonly KnownClientStore clientStore;
        private readonly IAMQPQueueManager manager;

        public LobbyController(ILogger<LobbyController> logger, KnownClientStore clientStore, IAMQPQueueManager manager)
        {
            this.logger=logger;
            this.clientStore=clientStore;
            this.manager=manager;
        }

        [HttpGet("connect")]
        public async Task<string> ConnectToLobby(string lobbyId)
        {
            this.logger.LogInformation(this.User.ExtractDisplayName());
            this.logger.LogInformation(this.User.ExtractIdentifier());

            var identifier = this.User.ExtractIdentifier();

            if (identifier == null)
            {
                identifier = Guid.NewGuid().ToString();
            }

            try
            {
                await this.manager.CreateQueueAsync(identifier);
            }
            catch (ArgumentException)
            {
                this.logger?.LogInformation("Connected to existing queue id: {}", identifier);
            }

            // send information to game client

            this.clientStore.Add(identifier);
            return identifier;
        }

        // TODO wir brauchen irgend ein disconnect welches bevor dem schließen gesendet wird oder wir machen alles über timeouts....
    }
}
