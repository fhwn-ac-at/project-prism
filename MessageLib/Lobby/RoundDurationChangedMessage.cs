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

    public class RoundDurationChangedMessageBody(int duration) : IMessageBody
    {
        [JsonProperty("duration")]
        [Range(0, 500)]
        [Required]
        public int Duration { get; init; } = duration;
    }
}
