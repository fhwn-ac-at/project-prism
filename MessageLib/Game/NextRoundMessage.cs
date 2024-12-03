namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;

    public class NextRoundMessage : Message<EmptyMessageBody>
    {
        public NextRoundMessage() : base(new EmptyMessageBody(), new MessageHeader(MessageType.nextRound))
        {
        }

        [JsonConstructor]
        public NextRoundMessage(MessageHeader header) : base(new EmptyMessageBody(), header)
        {
        }
    }
}
