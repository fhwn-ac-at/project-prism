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

    public class DrawingSizeChangedMessageBody(double size) : IMessageBody
    {
        private readonly double size = size;

        [JsonProperty("size")]
        [Range(0, 100)]
        public double Size { get => size; }
    }
}
