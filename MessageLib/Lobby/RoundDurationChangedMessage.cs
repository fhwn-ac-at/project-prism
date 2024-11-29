namespace MessageLib.Lobby
{
    using Newtonsoft.Json;

    public class RoundDurationChangedMessage(RoundDurationChangedMessageBody messageBody) : Message<RoundDurationChangedMessageBody>
    {
        private readonly MessageHeader header = new MessageHeader(MessageType.roundDurationChanged, DateTime.Now);

        private readonly RoundDurationChangedMessageBody body = messageBody;

        [JsonConstructor]
        public RoundDurationChangedMessage(RoundDurationChangedMessageBody messageBody, MessageHeader messageHeader) : this(messageBody)
        {
            this.header = messageHeader;
        }

        public override MessageHeader MessageHeader { get => header; } 

        public override RoundDurationChangedMessageBody MessageBody { get => body; }
    }
}
