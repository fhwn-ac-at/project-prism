namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System;

    public class ClosePathMessage() : Message<EmptyMessageBody>
    {
        private readonly MessageHeader header = new MessageHeader(MessageType.closePath);

        [JsonConstructor]
        public ClosePathMessage(MessageHeader header) : this()
        {
            this.header = header;
        }

        public override MessageHeader MessageHeader
        {
            get => header;
        }

        public override EmptyMessageBody MessageBody
        {
            get => new EmptyMessageBody();
        }
    }
}
