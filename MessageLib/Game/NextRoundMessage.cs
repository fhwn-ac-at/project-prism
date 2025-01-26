namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class NextRoundMessage : Message<NextRoundMessageBody>
    {
        public NextRoundMessage(NextRoundMessageBody body) : base(body, new MessageHeader(MessageType.nextRound))
        {
        }

        [JsonConstructor]
        public NextRoundMessage(NextRoundMessageBody body, MessageHeader header) : base(body, header)
        {
        }
    }

    public class NextRoundMessageBody(string word, int round, Dictionary<string, uint> score) : IMessageBody
    {
        private readonly string word = word;
        private readonly int round = round;
        private readonly Dictionary<string, uint> score = score;

        [JsonProperty("word")]
        public string Word
        {
            get
            {
                return this.word;
            }
        }

        [JsonProperty("round")]
        [Range(0, int.MaxValue)]
        public int Round
        {
            get
            {
                return this.round;
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
