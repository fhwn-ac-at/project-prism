namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;

    public class SetDrawerMessage : Message<SetDrawerMessageBody>
    {
        public SetDrawerMessage(SetDrawerMessageBody body) : base(body, new MessageHeader(MessageType.setDrawer))
        {
            
        }


        [JsonConstructor]
        public SetDrawerMessage(SetDrawerMessageBody body, MessageHeader header) : base(body, header)
        {
        }
    }

    public class SetDrawerMessageBody(IList<SelectWordItem> wordList) : IMessageBody
    {
        private readonly IList<SelectWordItem> wordList = wordList;

        [JsonProperty("words")]
        public IList<SelectWordItem> WordList { get => wordList; }
    }

    public class SelectWordItem
    {
        private readonly string word;
        private readonly byte difficulty;

        public SelectWordItem(string word, byte difficulty)
        {
            if (difficulty < 0 || difficulty > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(difficulty));
            }
            
            this.word = word;
            this.difficulty = difficulty;
        }

        [JsonProperty("word")]
        public string Word { get => word; }

        [JsonProperty("difficulty")]
        [Range(0, 2)]
        public byte Difficulty { get => difficulty; }
    }
}
