namespace GameService
{
    using AMQPLib;
    using FrenziedMarmot.DependencyInjection;
    using MessageLib;
    using MessageLib.SharedObjects;
    using System;


    [Injectable(Lifetime = ServiceLifetime.Singleton)]
    public class LobbyManager
    {
        private readonly Dictionary<string, GameLobby> lobbies = [];
        private readonly HashSet<string> users = [];
        private readonly IServiceProvider serviceProvider;

        public LobbyManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void ConnectUserToLobby(string lobbyId, User user)
        {
            ArgumentNullException.ThrowIfNull(lobbyId);

            ArgumentNullException.ThrowIfNull(user);


            if (!this.lobbies.ContainsKey(lobbyId))
            {
                this.lobbies.Add(
                    lobbyId,
                    new GameLobby(
                        lobbyId,
                        this.serviceProvider.GetRequiredService<IAMQPBroker>(),
                        this.serviceProvider,
                        this.serviceProvider.GetRequiredService<ILogger<GameLobby>>()
                        )
                    );
            }

            this.users.Add(user.Id);
            this.lobbies[lobbyId].AddUser(user);
        }

        public void DisconnectUserFromLobby(string lobbyId, string userId)
        {
            ArgumentNullException.ThrowIfNull(lobbyId);

            ArgumentNullException.ThrowIfNull(userId);


            if (!this.lobbies.TryGetValue(lobbyId, out GameLobby? value))
            {
                throw new ArgumentException(lobbyId);
            }

            value.RemoveUser(userId);

            if (value.UserCount<=0)
            {
                this.lobbies.Remove(lobbyId);
            }
        }

        public bool StartGame(string lobbyId)
        {
            ArgumentNullException.ThrowIfNull(lobbyId);

            if (!this.lobbies.TryGetValue(lobbyId, out GameLobby? value))
            {
                throw new ArgumentException(lobbyId);
            }

            return value.StartGame();
        }
    }
}
