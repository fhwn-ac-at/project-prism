namespace MessageLib.Lobby
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class RoundDurationChangedMessage : Message<RoundDurationChangedMessageBody> 
    {
        public RoundDurationChangedMessage(RoundDurationChangedMessageBody messageBody) : base(messageBody, new MessageHeader(MessageType.roundDurationChanged))
        {
            
        }

        [JsonConstructor]
        public RoundDurationChangedMessage(RoundDurationChangedMessageBody messageBody, MessageHeader messageHeader) : base(messageBody, messageHeader)
        { }
    }

    public class RoundDurationChangedMessageBody: IMessageBody
    {
        private readonly int duration;

        public RoundDurationChangedMessageBody(int duration)
        {
            if (duration < 0 || duration > 500)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            this.duration = duration;
        }

        [JsonProperty("duration")]
        [Range(0, 500)]
        [Required]
        public int Duration { get => duration; }
    }
}
