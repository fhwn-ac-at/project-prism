namespace BackendApi.Controllers
{
    public class ConnectResponse
    {
        public required string LobbyId { get; init; }

        public required string UserId { get; init; }

        public required string UserName { get; init; }
    }
}
