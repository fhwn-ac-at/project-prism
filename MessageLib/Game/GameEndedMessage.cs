namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System;

    public class GameEndedMessage : Message<EmptyMessageBody>
    {
        public GameEndedMessage() : base(new EmptyMessageBody(), new MessageHeader(MessageType.gameEnded))
        {
        }

        [JsonConstructor]
        public GameEndedMessage(MessageHeader header) : base(new EmptyMessageBody(), header)
        {
        }
    }
}
