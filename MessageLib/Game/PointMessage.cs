﻿namespace MessageLib.Game
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

    public class PointMessageBody : IMessageBody
    {
        private readonly RelativePoint point;
        private readonly HexColor color;
        private readonly double size;

        public PointMessageBody(RelativePoint point, HexColor color, double size)
        {
            if (size<0||size>100)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            this.point=point;
            this.color=color;
            this.size=size;
        }

        [JsonProperty("point")]
        public RelativePoint Point
        {
            get
            {
                return this.point;
            }
        }

        [JsonProperty("color")]
        public HexColor Color
        {
            get
            {
                return this.color;
            }
        }

        [JsonProperty("size")]
        [Range(0, 100)]
        public double Size
        {
            get
            {
                return this.size;
            }
        }
    }
}
