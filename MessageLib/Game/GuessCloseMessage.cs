namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class GuessCloseMessage : Message<GuessCloseMessageBody>
    {
        public GuessCloseMessage(GuessCloseMessageBody body) : base(body, new MessageHeader(MessageType.guessClose))
        {

        }

        [JsonConstructor]
        public GuessCloseMessage(GuessCloseMessageBody body, MessageHeader header) : base(body, header)
        {
        }
    }

    public class GuessCloseMessageBody(string guess, int distance) : IMessageBody
    {
        private readonly string guess = guess;
        private readonly int distance = distance;

        [JsonProperty("guess")]
        public string Guess
        {
            get
            {
                return this.guess;
            }
        }

        [JsonProperty("distance")]
        [Range(0, int.MaxValue)]
        public int Distance
        {
            get
            {
                return this.distance;
            }
        }
    }
}
