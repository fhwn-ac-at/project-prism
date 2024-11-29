namespace MessageLib.Lobby
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;

    public class RoundAmountChangedMessage(RoundAmountChangedMessageBody messageBody) : Message<RoundAmountChangedMessageBody>
    {
        private readonly MessageHeader header = new MessageHeader(MessageType.roundAmountChanged);

        private readonly RoundAmountChangedMessageBody body = messageBody;

        [JsonConstructor]
        public RoundAmountChangedMessage(RoundAmountChangedMessageBody messageBody, MessageHeader messageHeader) : this(messageBody)
        {
            this.header = messageHeader;
        }

        public override MessageHeader MessageHeader { get => header; } 

        public override RoundAmountChangedMessageBody MessageBody { get => body; }
    }
}
