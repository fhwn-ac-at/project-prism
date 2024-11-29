namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System;

    public class SetNotDrawerMessage : Message<EmptyMessageBody>
    {
        public SetNotDrawerMessage() : base(new EmptyMessageBody(), new MessageHeader(MessageType.setNotDrawer))
        {
        }

        [JsonConstructor]
        public SetNotDrawerMessage(MessageHeader header) : base(new EmptyMessageBody(), header)
        {
        }
    }
}
