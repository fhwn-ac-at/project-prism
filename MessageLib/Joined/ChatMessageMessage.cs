namespace MessageLib.Joined
{
    using MessageLib;
    using MessageLib.SharedObjects;

    public class ChatMessageMessage(ChatMessageMessageBody body) : Message<ChatMessageMessageBody>
    {
        private readonly ChatMessageMessageBody body = body;

        private readonly MessageHeader header = new MessageHeader(MessageType.chatMessage);

        public override MessageHeader MessageHeader
        {
            get => header;
        }

        public override ChatMessageMessageBody MessageBody
        {
            get => body;
        }
    }
}
