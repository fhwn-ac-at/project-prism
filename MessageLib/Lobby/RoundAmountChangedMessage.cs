namespace MessageLib.Lobby
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class RoundAmountChangedMessage : Message<RoundAmountChangedMessageBody>
    {
        public RoundAmountChangedMessage(RoundAmountChangedMessageBody messageBody) : base(messageBody, new MessageHeader(MessageType.roundAmountChanged))
        {

        }

        [JsonConstructor]
        public RoundAmountChangedMessage(RoundAmountChangedMessageBody messageBody, MessageHeader messageHeader) : base(messageBody, messageHeader)
        { }
    }

    public class RoundAmountChangedMessageBody : IMessageBody
    {
        private readonly int rounds;

        public RoundAmountChangedMessageBody(int rounds)
        {
            if (rounds<0||rounds>50)
            {
                throw new ArgumentOutOfRangeException(nameof(rounds));
            }

            this.rounds=rounds;
        }

        [JsonProperty("rounds")]
        [Range(0, 50)]
        [Required]
        public int Rounds
        {
            get
            {
                return this.rounds;
            }
        }
    }
}
