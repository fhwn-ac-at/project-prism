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
        private readonly HashSet<string> users;
        // Users that where connected once
        private readonly HashSet<string> zombieUsers = [];

        private readonly LinkedList<Message<IMessageBody>> drawing = [];
        private string? selectedWord;

        private readonly int roundDuration;
        private int roundAmount;

        public bool Running { get; internal set; } = false;
        public string? DrawerId { get; internal set; }
        public DateTime? RoundStartTime { get; internal set; }

        public int RoundDuration => roundDuration;
        
        public Game(HashSet<string> users, int roundAmount, int roundDuration)
        {
            this.users = users;
            this.roundAmount = roundAmount;
            this.roundDuration = roundDuration;
        }

        public bool AddUser(string key)
        {
            this.zombieUsers.Remove(key);
            return users.Add(key);   
        }

        public bool RemoveUser(string key)
        {
            this.zombieUsers.Add(key);
            return users.Remove(key);
        }

        public void AddToDrawing<T>(Message<T> e) where T : IMessageBody
        {
            //this.drawing.AddLast(e);
        }

        public void ClearDrawing()
        {
            this.drawing.Clear();
        }

        public bool GuessWord(string text)
        {
            return this.selectedWord != null && text==this.selectedWord;
        }

        public bool SelectWord(string word)
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            this.drawing.RemoveLast();
        }
    }
}
