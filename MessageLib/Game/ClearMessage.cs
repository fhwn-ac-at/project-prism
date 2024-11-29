namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System;

    public class ClearMessage() : Message<EmptyMessageBody>
    {
        private readonly MessageHeader header = new MessageHeader(MessageType.clear);

        [JsonConstructor]
        public ClearMessage(MessageHeader header) : this()
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
