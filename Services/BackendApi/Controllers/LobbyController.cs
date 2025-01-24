namespace BackendApi.Controllers
{
    using AMQPLib;
    using BackendApi.ApiClients;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LobbyController : ControllerBase
    {
        private readonly ILogger<LobbyController> logger;
        private readonly KnownClientStore clientStore;
        private readonly IAMQPQueueManager manager;
        private readonly GeneratedGameClient gameServiceClient;

        public LobbyController(ILogger<LobbyController> logger, KnownClientStore clientStore, IAMQPQueueManager manager, GeneratedGameClientFactory gameServiceClient)
        {
            this.logger=logger;
            this.clientStore=clientStore;
            this.manager=manager;
            this.gameServiceClient=gameServiceClient.Generate();
        }

        [HttpGet("connect")]
        public async Task<User> ConnectToLobby(string lobbyId)
        {
            this.logger.LogInformation(this.User.ExtractDisplayName());
            this.logger.LogInformation(this.User.ExtractIdentifier());

            var identifier = this.User.ExtractIdentifier();

            if (identifier == null)
            {
                identifier = Guid.NewGuid().ToString();
            }

            User user = new User();

            user.Id=identifier;
            user.Name=this.User.ExtractDisplayName();
            ;
            if (this.clientStore.TryGetValue(identifier, out string? connectedLobbyId) && connectedLobbyId == lobbyId)
            {
                this.logger?.LogInformation("Already connected to lobby. lobby: {} user: {}", lobbyId, identifier);
                return user;
            }

            try
            {
                await this.manager.CreateQueueAsync(identifier);
            }
            catch (ArgumentException)
            {
                this.logger?.LogInformation("Connected to existing queue id: {}", identifier);
            }
            await this.gameServiceClient.ConnectUserToLobbyAsync(lobbyId, user);

            // send information to game client

            this.clientStore.Add(identifier, lobbyId);
            return user;
        }

        // TODO wir brauchen irgend ein disconnect welches bevor dem schließen gesendet wird oder wir machen alles über timeouts....

        [HttpGet("startGame")]
        public async Task StartGame(string lobbyId)
        {
            try
            {
                await this.gameServiceClient.StartGameAsync(lobbyId);
            }
            catch (ApiException ex)
            {
                this.logger?.LogWarning(ex.Message);
                throw new BadHttpRequestException("Could not be started");
            }
        }
    }
}
