namespace BackendApi.Controllers
{
    using AMQPLib;
    using BackendApi.ApiClients;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LobbyController : ControllerBase
    {
        private readonly ILogger<LobbyController> logger;
        private readonly KnownClientStore clientStore;
        private readonly GeneratedGameClientFactory gameServiceClientFactory;
        private readonly GeneratedGameClient gameServiceClient;
        private readonly ushort connectionConfirmThreshold;

        public LobbyController(ILogger<LobbyController> logger, KnownClientStore clientStore, GeneratedGameClientFactory gameServiceClientFactory, IOptions<ConnectionConfirmOptions> connectionConfirmOptions)
        {
            ArgumentNullException.ThrowIfNull(connectionConfirmOptions);

            this.logger=logger;
            this.clientStore=clientStore;
            this.gameServiceClientFactory=gameServiceClientFactory;
            this.gameServiceClient=gameServiceClientFactory.Generate();
            this.connectionConfirmThreshold=connectionConfirmOptions.Value.ConfirmationThreshold;
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

            User user = new User
            {
                Id=identifier,
                Name=this.User.ExtractDisplayName()
            };

            if (this.clientStore.TryGetValue(identifier, out KnownClientInfo? clientInfo))
            {
                this.logger?.LogInformation("Already connected. user: {}", identifier);
                //return this.BadRequest("Already connected.");
                throw new BadHttpRequestException("Already connected");
            }

            try
            {
                await this.gameServiceClient.ConnectUserToLobbyAsync(lobbyId, user);
            }
            catch (ArgumentException)
            {
                this.logger?.LogInformation("Connected to existing queue id: {}", identifier);
            }
            catch (Exception ex)
            {
                this.logger?.LogError("The user could not be connected. Error message: {}", ex.Message);
                throw new BadHttpRequestException("Could not connect user");
            }

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            this.clientStore.TryAdd(identifier, new KnownClientInfo { lobbyId=lobbyId, token=tokenSource });

            _=Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(this.connectionConfirmThreshold, tokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    this.logger?.LogTrace("Connection for user: {}, lobby: {} was confirmed", user.Id, lobbyId);
                    return;
                }

                this.logger?.LogTrace("Disconnecting user as no web socket connection was created. user: {}, lobby: {}", user.Id, lobbyId);
                try
                {
                    await this.gameServiceClientFactory.Generate().DisconnectUserFromLobbyAsync(lobbyId, user.Id);
                }
                catch (Exception ex)
                {
                    this.logger?.LogError("Could not disconnect user {} from lobby {} because of {}", user.Id, lobbyId, ex.Message);
                }
                if (!this.clientStore.Remove(identifier, out KnownClientInfo? value))
                {
                    this.logger?.LogWarning("Couldn't remove User {} from known clients", identifier);
                }
                this.logger?.LogTrace("User {} disconnected", user.Id);
            });
            return user;
        }

        [HttpGet("startGame")]
        public async Task<IActionResult> StartGame(string lobbyId)
        {
            try
            {
                await this.gameServiceClient.StartGameAsync(lobbyId);
                return this.Ok();
            }
            catch (ApiException ex)
            {
                this.logger?.LogWarning(ex.Message);
            }

            return this.BadRequest("Could not be started");
        }
    }
}
