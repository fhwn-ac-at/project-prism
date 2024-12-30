namespace GameLib
{
    using System.Collections.Generic;

    public class Lobby
    {
        private readonly HashSet<string> users = [];

        private readonly string id;

        public Lobby(string id)
        {
            this.id = id;
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
            return new Game(this.users, this.RoundAmount, this.RoundDuration);
        }
    }
}
