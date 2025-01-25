namespace GameLib
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;

    public class Lobby
    {
        private readonly HashSet<string> users = [];

        private readonly IServiceProvider serviceProvider;

        public Lobby(IServiceProvider serviceProvider, IOptions<LobbyOptions> lobbyOptions)
        {
            ArgumentNullException.ThrowIfNull(nameof(lobbyOptions));
            this.serviceProvider = serviceProvider;
            this.RoundAmount=lobbyOptions.Value.DefaultRoundAmount;
            this.RoundDuration=lobbyOptions.Value.DefaultRoundDuration;
        }

        public int UserCount => users.Count;

        public IEnumerable<string> Users => users;

        public int RoundAmount { get; set; }

        public int RoundDuration { get; set; }

        public bool AddUser(string key)
        {
            return users.Add(key);   
        }

        public bool RemoveUser(string key)
        {
            return users.Remove(key);
        }

        public Game StartGame()
        {
            return new Game(
                this.users, 
                this.RoundAmount,
                this.RoundDuration, 
                this.serviceProvider.GetRequiredService<WordList>(),
                this.serviceProvider.GetRequiredService<IOptions<GameOptions>>()
            );
        }
    }
}
