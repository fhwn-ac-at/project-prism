namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System;

    public class SelectWordMessage : Message<SelectWordMessageBody>
    {
        public SelectWordMessage(SelectWordMessageBody body) : base(body, new MessageHeader(MessageType.selectWord))
        {
            
        }


        [JsonConstructor]
        public SelectWordMessage(SelectWordMessageBody body, MessageHeader header) : base(body, header)
        {
        }
    }

    public class SelectWordMessageBody(string word) : IMessageBody
    {
        private readonly string word = word;

        [JsonProperty("word")]
        public string Word { get => word; }
    }
}
