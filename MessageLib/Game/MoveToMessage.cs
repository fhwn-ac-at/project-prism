namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;

    public class MoveToMessage : Message<MoveToMessageBody>
    {
        public MoveToMessage(MoveToMessageBody body) : base(body, new MessageHeader(MessageType.moveTo))
        {
            
        }

        [JsonConstructor]
        public MoveToMessage(MoveToMessageBody body, MessageHeader header) : base(body, header)
        {
            
        }
    }

    public class MoveToMessageBody(RelativePoint point) : IMessageBody
    {
        private readonly RelativePoint point = point;

        [JsonProperty("point")]
        public RelativePoint Point { get => point; }
    }
}
