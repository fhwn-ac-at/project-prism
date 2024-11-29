namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;

    public class UndoMessage : Message<EmptyMessageBody>
    {
        public UndoMessage() : base(new EmptyMessageBody(), new MessageHeader(MessageType.undo))
        {
        }

        [JsonConstructor]
        public UndoMessage(MessageHeader header) : base(new EmptyMessageBody(), header)
        {
        }
    }
}
