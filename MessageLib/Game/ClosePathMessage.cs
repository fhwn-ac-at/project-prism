namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System;

    public class ClosePathMessage : Message<EmptyMessageBody>
    {
        public ClosePathMessage() : base(new EmptyMessageBody(), new MessageHeader(MessageType.closePath))
        {
        }

        [JsonConstructor]
        public ClosePathMessage(MessageHeader header) : base(new EmptyMessageBody(), header)
        {
        }
    }
}
