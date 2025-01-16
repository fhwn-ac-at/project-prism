namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DrawingSizeChangedMessage : Message<DrawingSizeChangedMessageBody>
    {
        public DrawingSizeChangedMessage(DrawingSizeChangedMessageBody body) : base(body, new MessageHeader(MessageType.drawingSizeChanged))
        {

        }

        [JsonConstructor]
        public DrawingSizeChangedMessage(DrawingSizeChangedMessageBody body, MessageHeader header) : base(body, header)
        {

        }
    }

    public class DrawingSizeChangedMessageBody : IMessageBody
    {
        private readonly double size;

        public DrawingSizeChangedMessageBody(double size)
        {
            if (size<0||size>100)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            this.size=size;
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
