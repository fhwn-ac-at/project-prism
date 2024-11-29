namespace MessageLib
{
    using MessageLib.Game;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;


    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Message<T>(T body, MessageHeader header) where T : IMessageBody
    {
        private readonly MessageHeader header = header;
        private readonly T body = body;

        [JsonProperty("header")]
        [Required]
        public MessageHeader MessageHeader { get => this.header; }

        [JsonProperty("body")]
        [Required]
        public T MessageBody { get => this.body; }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
