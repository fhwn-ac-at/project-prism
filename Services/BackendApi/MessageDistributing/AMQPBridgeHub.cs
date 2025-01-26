namespace BackendApi.MessageDistributing;

using AMQPLib;
using BackendApi;
using BackendApi.ApiClients;
using MessageLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;


//[Authorize]
public class AMQPBridgeHub : Hub
{
    private readonly ILogger<AMQPBridgeHub>? logger;

    private readonly IServiceProvider serviceProvider;
    private readonly KnownConnectionsStore connectors;
    private readonly KnownClientStore clientStore;
    private readonly GeneratedGameClient generatedGameClient;

    public AMQPBridgeHub(IServiceProvider serviceProvider, KnownClientStore clientStore, GeneratedGameClientFactory generatedGameClient, KnownConnectionsStore connectors, ILogger<AMQPBridgeHub> logger)
    {
        this.serviceProvider=serviceProvider;
        this.clientStore=clientStore;
        this.generatedGameClient=generatedGameClient.Generate();
        this.logger =logger;
        this.connectors=connectors;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = this.Context.GetHttpContext();
        var clientId = httpContext?.Request.RouteValues["client-id"]?.ToString();

        if (clientId == null)
        {
            clientId=httpContext?.Request.Path.Value?.Split('/').Last();
        }

        if (clientId==null)
        {
            this.logger?.LogError("Websocket connected without client id");
            this.Context.Abort();
            return;
            //throw new BadHttpRequestException("Missing client id");
        }

        if (!this.clientStore.TryGetValue(clientId, out KnownClientInfo? clientInfo))
        {
            this.logger?.LogError("Websocket connected on unknown client id: {}", clientId);
            this.Context.Abort();
            return;
            //throw new BadHttpRequestException("Unknown client id");
        }

        clientInfo.token.Cancel();

        if (this.Context.User==null)
        {
            this.logger?.LogError("User connected without claim.");
            this.Context.Abort();
            return;
            //throw new BadHttpRequestException("No Token");
        }

        this.logger?.LogTrace("Extracted display name: {}", this.Context.User?.ExtractDisplayName());
        this.logger?.LogTrace("Extracted user identifier: {}", this.Context.User?.ExtractIdentifier());

        var identifier = this.Context.User?.ExtractIdentifier();

        if (identifier==null)
        {
            identifier=clientId;
        }
        else if (identifier!=clientId)
        {
            this.logger?.LogError("Connection on id {} doesn't match the user id {}.", clientId, identifier);
            this.Context.Abort();
            return;
            //throw new BadHttpRequestException("Invalid client id");
        }

        var clientProxy = this.Clients.Client(this.Context.ConnectionId);
        var connector = new WebsocketToAMQPConnector(
            clientProxy,
            identifier,
            this.serviceProvider.GetRequiredService<IAMQPBroker>(),
            this.serviceProvider.GetRequiredService<PlainMessageDistributor>(),
            this.serviceProvider.GetRequiredService<Validator>(),
            this.serviceProvider.GetRequiredService<ILogger<WebsocketToAMQPConnector>>()
            );

        if (!this.connectors.TryAdd(this.Context.ConnectionId, connector))
        {
            this.logger?.LogWarning("Couldn't store connector with id {}", this.Context.ConnectionId);
        };

        this.logger?.LogTrace("Stored connector with id {}", this.Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var clientId = this.Context.GetHttpContext()?.Request.RouteValues["client-id"]?.ToString();

        if (clientId==null)
        {
            this.logger?.LogError("Websocket disconnected without lobby id. Exception: {}", exception);
            this.Context.Abort();
            return;
            //throw new BadHttpRequestException("Missing client id");
        }

        if (!this.clientStore.TryGetValue(clientId, out KnownClientInfo? clientInfo))
        {
            this.logger?.LogError("Websocket disconnected on unknown client id: {} Exception: {}", clientId, exception);
            this.Context.Abort();
            return;
            //throw new BadHttpRequestException("Unknown client id");
        }

        this.clientStore.Remove(clientId, out KnownClientInfo? _);

        if (this.Context.User==null)
        {
            this.logger?.LogError("User disconnected without claim. Exception: {}", exception);
            this.Context.Abort();
            return;
            //throw new BadHttpRequestException("No Token");
        }

        var identifier = this.Context.User?.ExtractIdentifier();

        if (identifier==null)
        {
            identifier=clientId;
        }
        else if (identifier!=clientId)
        {
            this.logger?.LogError("Disconnected connection on id {} doesn't match the user id {}. Exception: {}", clientId, identifier, exception);
            this.Context.Abort();
            return;
            //throw new BadHttpRequestException("Invalid client id");
        }

        try
        {
            await this.generatedGameClient.DisconnectUserFromLobbyAsync(clientInfo.lobbyId, identifier);
        }
        catch (Exception e)
        {
            this.logger?.LogError("Couldn't disconnect User from Lobby. Error message: {}", e.Message);
        }

        if (this.connectors.Remove(this.Context.ConnectionId, out var _))
        {
            this.logger?.LogWarning("Couldn't remove connector {} from Dictionary", this.Context.ConnectionId);
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task Backend(string message)
    {
        if (!this.connectors.TryGetValue(this.Context.ConnectionId, out var connector))
        {
            this.logger?.LogError("Connection with id {} not known", this.Context.ConnectionId);
            this.Context.Abort();
            return;
            //throw new BadHttpRequestException("Invalid connection");
        }

        await connector.SendAsync(message);
    }
}