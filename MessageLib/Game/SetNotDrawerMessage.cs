namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System;

    public class SetNotDrawerMessage() : Message<EmptyMessageBody>
    {
        private readonly MessageHeader header = new MessageHeader(MessageType.undo);

        [JsonConstructor]
        public SetNotDrawerMessage(MessageHeader header) : this()
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
