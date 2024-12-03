namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;

    public class LineToMessage : Message<LineToMessageBody>
    {
        public LineToMessage(LineToMessageBody body) : base(body, new MessageHeader(MessageType.lineTo))
        {
            
        }

        [JsonConstructor]
        public LineToMessage(LineToMessageBody body, MessageHeader header) : base(body, header)
        {
            
        }
    }

    public class LineToMessageBody(RelativePoint point, HexColor color) : IMessageBody
    {
        private readonly RelativePoint point = point;
        private readonly HexColor color = color;

        [JsonProperty("point")]
        public RelativePoint Point { get => point; }

        [JsonProperty("color")]
        public HexColor Color { get => color; }
    }
}
