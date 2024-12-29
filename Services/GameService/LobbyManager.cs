namespace GameService
{
    using AMQPLib;
    using FrenziedMarmot.DependencyInjection;
    using MessageLib.SharedObjects;
    using System;


    [Injectable(Lifetime = ServiceLifetime.Singleton)]
    public class LobbyManager
    {
        private readonly Dictionary<string, GameService> lobbies = [];
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
                this.lobbies.Add(lobbyId, this.serviceProvider.GetRequiredService<GameService>());
            }

            this.users.Add(user.Id);
            this.lobbies[lobbyId].AddUser(user.Id);
        }

        public void DisconnectUserFromLobby(string lobbyId, User user)
        {
            ArgumentNullException.ThrowIfNull(lobbyId);

            ArgumentNullException.ThrowIfNull(user);


            if (!this.lobbies.TryGetValue(lobbyId, out GameService? value))
            {
                throw new ArgumentException(lobbyId);
            }

            value.RemoveUser(user.Id);

            if (value.UserCount<=0)
            {
                this.lobbies.Remove(lobbyId);
            }
        }
    }
}
