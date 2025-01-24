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
        var clientId = this.Context.GetHttpContext()?.Request.RouteValues["client-id"]?.ToString();

        if (clientId==null)
        {
            this.logger?.LogError("Websocket connected without client id");
            throw new BadHttpRequestException("Missing client id");
        }

        if (!this.clientStore.ContainsKey(clientId))
        {
            this.logger?.LogError("Websocket connected on unknown client id: {}", clientId);
            throw new BadHttpRequestException("Unknown client id");
        }


        if (this.Context.User==null)
        {
            this.logger?.LogError("User connected without claim.");
            throw new BadHttpRequestException("No Token");
        }

        this.logger?.LogInformation(this.Context.User?.ExtractDisplayName());
        this.logger?.LogInformation(this.Context.User?.ExtractIdentifier());

        var identifier = this.Context.User?.ExtractIdentifier();

        if (identifier==null)
        {
            identifier=clientId;
        }
        else if (identifier!=clientId)
        {
            this.logger?.LogError("Connection on id {} doesn't match the user id {}.", clientId, identifier);
            throw new BadHttpRequestException("Invalid client id");
        }

        // TODO what to do if the connection was not recreated on the request and the websocket connects????

        var clientProxy = Clients.Client(this.Context.ConnectionId);
        var connector = new WebsocketToAMQPConnector(
            clientProxy,
            identifier,
            this.serviceProvider.GetRequiredService<IAMQPBroker>(),
            this.serviceProvider.GetRequiredService<PlainMessageDistributor>(),
            this.serviceProvider.GetRequiredService<Validator>(),
            this.serviceProvider.GetRequiredService<ILogger<WebsocketToAMQPConnector>>()
            );

        this.connectors.Add(this.Context.ConnectionId, connector);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var clientId = this.Context.GetHttpContext()?.Request.RouteValues["client-id"]?.ToString();

        if (clientId==null)
        {
            this.logger?.LogError("Websocket connect without lobby id. Exception: {}", exception);
            throw new BadHttpRequestException("Missing client id");
        }

        if (!this.clientStore.TryGetValue(clientId, out string? lobbyId))
        {
            this.logger?.LogError("Websocket connected on unknown client id: {} Exception: {}", clientId, exception);
            throw new BadHttpRequestException("Unknown client id");
        }

        this.clientStore.Remove(clientId);

        if (this.Context.User==null)
        {
            this.logger?.LogError("User disconnected without claim. Exception: {}", exception);
            throw new BadHttpRequestException("No Token");
        }

        var identifier = this.Context.User?.ExtractIdentifier();

        if (identifier==null)
        {
            identifier=clientId;
        }
        else if (identifier!=clientId)
        {
            this.logger?.LogError("Connection on id {} doesn't match the user id {}. Exception: {}", clientId, identifier, exception);
            throw new BadHttpRequestException("Invalid client id");
        }

        try
        {
            await this.generatedGameClient.DisconnectUserFromLobbyAsync(lobbyId, identifier);
        }
        catch (Exception e)
        {
            this.logger?.LogError("Couldn't disconnect User from Lobby. Error message: {}", e.Message);
        }

        this.connectors.Remove(this.Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task Backend(string message)
    {
        this.logger?.LogError(this.GetHashCode().ToString());
        if (!this.connectors.TryGetValue(Context.ConnectionId, out var connector))
        {
            this.logger?.LogError("Connection with id {} not known", Context.ConnectionId);
            throw new BadHttpRequestException("Invalid connection");
        }

        await connector.SendAsync(message);
    }
}