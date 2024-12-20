namespace BackendApi;

using AMQPLib;
using MessageLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;

[Authorize]
public class AMQPBridgeHub : Hub
{
    private readonly ILogger<AMQPBridgeHub>? logger;

    private readonly IServiceProvider serviceProvider;
    private readonly Dictionary<string, WebsocketToAMQPConnector> connectors = [];
    private readonly KnownClientStore clientStore;

    public AMQPBridgeHub(IServiceProvider serviceProvider, KnownClientStore clientStore)
    {
        this.serviceProvider=serviceProvider;
        this.clientStore=clientStore;
    }

    public override async Task OnConnectedAsync()
    {
        var clientId = this.Context.GetHttpContext()?.Request.RouteValues["client-id"]?.ToString();

        if (clientId== null)
        {
            this.logger?.LogError("Websocket connected without client id");
            throw new BadHttpRequestException("Missing client id");
        }

        if (!this.clientStore.Contains(clientId))
        {
            this.logger?.LogError("Websocket connected on unknown client id: {}", clientId);
            throw new BadHttpRequestException("Unknown client id");
        }


        if (this.Context.User == null)
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
        } else if (identifier!=clientId)
        {
            this.logger?.LogError("Connection on id {} doesn't match the user id {}.", clientId, identifier);
            throw new BadHttpRequestException("Invalid client id");
        }

        // TODO what to do if the connection was not recreated on the request and the websocket connects????

        var clientProxy = Clients.User(this.Context.ConnectionId);
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

        if (!this.clientStore.Contains(clientId))
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

        // TODO maybe notify game service about disconnect 
        // Queue aber nicht löschen da es als fehler und nicht gewollt klassifiziert wird.

        this.connectors.Remove(this.Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    // TODO look up how to receive messages and send it based on the user claim
}