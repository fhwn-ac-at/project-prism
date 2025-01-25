namespace GameService.Controllers
{
    using AMQPLib;
    using MessageLib.SharedObjects;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class LobbyController : ControllerBase
    {
        private readonly LobbyManager lobbyManager;

        private readonly IAMQPQueueManager manager;
        private readonly ILogger<LobbyController>? logger;

        public LobbyController(LobbyManager lobbyManager, IAMQPQueueManager manager, ILogger<LobbyController>? logger = null)
        {
            this.lobbyManager = lobbyManager;
            this.manager = manager;
            this.logger=logger;
        }

        [HttpPost("connectUserToLobby")]
        public async Task ConnectUserToLobby(User user, string lobbyId)
        {
            try
            {
                await this.manager.CreateQueueAsync(user.Id);
            }
            catch (ArgumentException)
            {
                this.logger?.LogInformation("Connected to existing queue id: {}", user.Id);
            }

            this.logger?.LogDebug("User: {} connected to lobby: {}", user, lobbyId);
            this.lobbyManager.ConnectUserToLobby(lobbyId, user);
        }

        [HttpPost("startGame")]
        public IActionResult StartGame(string lobbyId)
        {
            if (!this.lobbyManager.StartGame(lobbyId))
            {
                this.logger?.LogWarning("Not enough players for lobby: {}", lobbyId);
                return this.BadRequest("Not enough player");
            }


            this.logger?.LogDebug("Started game for lobby: {}", lobbyId);
            return this.Ok();
        }

        [HttpDelete("disconnectUserFromLobby")]
        public async Task DisconnectUserFromLobby(string lobbyId, string userId)
        {
            var removeQueueTask = this.manager.RemoveQueueAsync(userId);

            this.logger?.LogDebug("User: {} disconnected from lobby: {}", userId, lobbyId);
            this.lobbyManager.DisconnectUserFromLobby(lobbyId, userId);
            await removeQueueTask;
        }
    }
}
