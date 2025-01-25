namespace GameLib
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;

    public class Lobby
    {
        private readonly HashSet<string> users = [];

        private readonly string id;

        private readonly IServiceProvider serviceProvider;

        public Lobby(string id, IServiceProvider serviceProvider)
        {
            this.id = id;
            this.serviceProvider = serviceProvider;
        }

        public int UserCount => users.Count;

        public IEnumerable<string> Users => users;

        // TODO get default values for this
        public int RoundAmount { get; set; } = 1;

        public int RoundDuration { get; set; } = 30;

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
