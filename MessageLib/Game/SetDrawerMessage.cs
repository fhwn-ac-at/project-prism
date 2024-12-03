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

    public class SelectWordItem(string word, byte difficulty)
    {
        private readonly string word = word;
        private readonly byte difficulty = difficulty;

        [JsonProperty("word")]
        public string Word { get => word; }

        [JsonProperty("difficulty")]
        [Range(0, 2)]
        public byte Difficulty { get => difficulty; }
    }
}
