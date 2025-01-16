namespace GameService.Controllers
{
    using MessageLib.SharedObjects;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class LobbyController : ControllerBase
    {
        private readonly LobbyManager lobbyManager;

        private readonly ILogger<LobbyController>? logger;

        public LobbyController(LobbyManager lobbyManager, ILogger<LobbyController>? logger = null)
        {
            this.lobbyManager = lobbyManager;
            this.logger=logger;
        }

        [HttpPost("connectUserToLobby")]
        public void ConnectUserToLobby(User user, string lobbyId)
        {
            this.logger?.LogDebug("User: {} connected to lobby: {}", user, lobbyId);
            this.lobbyManager.ConnectUserToLobby(lobbyId, user);
        }

        [HttpPost("startGame")]
        public void StartGame(string lobbyId)
        {
            this.logger?.LogDebug("Start game for lobby: {}", lobbyId);
            if (!this.lobbyManager.StartGame(lobbyId))
            {
                throw new BadHttpRequestException("Not enough player");
            }
        }

        [HttpDelete("disconnectUserFromLobby")]
        public void DisconnectUserFromLobby(User user, string lobbyId)
        {
            this.logger?.LogDebug("User: {} disconnected from lobby: {}", user, lobbyId);
            this.lobbyManager.DisconnectUserFromLobby(lobbyId, user);
        }
    }
}
