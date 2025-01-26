namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;

    public class GameEndedMessage : Message<GameEndedMessageBody>
    {
        public GameEndedMessage(GameEndedMessageBody body) : base(body, new MessageHeader(MessageType.gameEnded))
        {
        }

        [JsonConstructor]
        public GameEndedMessage(GameEndedMessageBody body, MessageHeader header) : base(body, header)
        {
        }
    }

    public class GameEndedMessageBody(string word, Dictionary<string, uint> score) : IMessageBody
    {
        private readonly string word = word;
        private readonly Dictionary<string, uint> score = score;

        [JsonProperty("word")]
        public string Word
        {
            get
            {
                return this.word;
            }
        }

        [JsonProperty("score")]
        public Dictionary<string, uint> Score
        {
            get
            {
                return this.score;
            }
        }
    }
}
