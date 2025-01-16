namespace MessageLib
{
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;


    [JsonObject(MemberSerialization.OptIn)]
    public class Message<T>(T body, MessageHeader header) where T : IMessageBody 
    {
        private MessageHeader header = header;
        private readonly T body = body;

        [JsonProperty("header")]
        [Required]
        public MessageHeader MessageHeader
        {
            get
            {
                return this.header;
            }
        }

        [JsonProperty("body")]
        [Required]
        public T MessageBody
        {
            get
            {
                return this.body;
            }
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void UpdateHeaderTimestamp()
        {
            this.header = new MessageHeader(this.header.Type);
        }
    }
}
