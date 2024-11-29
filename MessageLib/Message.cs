namespace MessageLib
{
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;


    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Message<T> where T : IMessageBody
    {
        [JsonProperty("header")]
        [Required]
        public abstract MessageHeader MessageHeader { get; }

        [JsonProperty("body")]
        [Required]
        public abstract T MessageBody { get; }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
