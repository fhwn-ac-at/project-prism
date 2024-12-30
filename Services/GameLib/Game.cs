namespace GameLib
{
    using FrenziedMarmot.DependencyInjection;
    using MessageLib;
    using MessageLib.SharedObjects;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    [Injectable(Lifetime = ServiceLifetime.Transient)]
    public class Game
    {
        private readonly Dictionary<string, UserGameState> users;
        // Users that where connected once
        private readonly Dictionary<string, UserGameState> zombieUsers = [];

        private readonly LinkedList<Message<IMessageBody>> drawing = [];
        private string? selectedWord;

        private readonly int roundDuration;
        private int roundAmount;
        private uint guessedCounter;

        public bool Running { get; internal set; } = false;
        public string? DrawerId { get; internal set; }
        public DateTime? RoundStartTime { get; internal set; }

        public int RoundDuration => roundDuration;
        
        internal Game(HashSet<string> users, int roundAmount, int roundDuration)
        {
            this.users = users.ToDictionary((key) => key, (_) => new UserGameState());
            this.roundAmount = roundAmount;
            this.roundDuration = roundDuration;
        }

        public void AddUser(string key)
        {
            if (!this.zombieUsers.TryGetValue(key, out var user))
            {
                this.users.Add(key, new());
                return;
            }

            this.zombieUsers.Remove(key);
            this.users.Add(key, new(user.Score));   
        }

        public void RemoveUser(string key)
        {
            if (!this.users.TryGetValue(key, out var user))
            {
                return;
            }
            
            this.zombieUsers.Add(key, new(user.Score));
            this.users.Remove(key);
        }

        public void AddToDrawing<T>(Message<T> e) where T : IMessageBody
        {
            //this.drawing.AddLast(e);
        }

        public void ClearDrawing()
        {
            this.drawing.Clear();
        }

        public bool GuessWord(string text, string key)
        {
            var guessed = this.selectedWord != null && text==this.selectedWord;

            if (guessed)
            {
                if (!this.users.TryGetValue(key, out var user))
                {
                    return false;
                }

                user.Guessed=true;
                this.guessedCounter++;
            }

            return guessed;
        }

        public bool SelectWord(string word)
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            this.drawing.RemoveLast();
        }

        private void RoundEnded()
        {

            this.guessedCounter=0;
            this.selectedWord=null;
            this.roundAmount--;
            this.ClearDrawing();
        }
    }
}
