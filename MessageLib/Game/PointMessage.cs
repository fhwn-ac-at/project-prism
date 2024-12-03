namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class PointMessage : Message<PointMessageBody>
    {
        public PointMessage(PointMessageBody body) : base(body, new MessageHeader(MessageType.point))
        {
            
        }

        [JsonConstructor]
        public PointMessage(PointMessageBody body, MessageHeader header) : base(body, header)
        {
            
        }
    }

    public class PointMessageBody(RelativePoint point, HexColor color, double size) : IMessageBody
    {
        private readonly RelativePoint point = point;
        private readonly HexColor color = color;
        private readonly double size = size;

        [JsonProperty("point")]
        public RelativePoint Point { get => point; }

        [JsonProperty("color")]
        public HexColor Color { get => color; }

        [JsonProperty("size")]
        [Range(0, 100)]
        public double Size { get => size; }
    }
}
