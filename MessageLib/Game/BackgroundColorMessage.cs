namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System;

    public class BackgroundColorMessage : Message<BackgroundColorMessageBody>
    {
        public BackgroundColorMessage(BackgroundColorMessageBody body) : base(body, new MessageHeader(MessageType.backgroundColor))
        {
            
        }


        [JsonConstructor]
        public BackgroundColorMessage(BackgroundColorMessageBody body, MessageHeader header) : base(body, header)
        {
        }
    }

    public class BackgroundColorMessageBody(HexColor hexColor) : IMessageBody
    {
        private readonly HexColor hexColor = hexColor;

        [JsonProperty("color")]
        public HexColor HexColor { get => hexColor; }
    }
}
