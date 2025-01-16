namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;

    public class GameStartedMessage : Message<EmptyMessageBody>
    {
        public GameStartedMessage() : base(new EmptyMessageBody(), new MessageHeader(MessageType.gameStarted))
        {
        }

        [JsonConstructor]
        public GameStartedMessage(MessageHeader header) : base(new EmptyMessageBody(), header)
        {
        }
    }
}
