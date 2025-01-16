namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;

    public class ClearMessage : Message<EmptyMessageBody>
    {
        public ClearMessage() : base(new EmptyMessageBody(), new MessageHeader(MessageType.clear))
        {
        }

        [JsonConstructor]
        public ClearMessage(MessageHeader header) : base(new EmptyMessageBody(), header)
        {
        }
    }
}
