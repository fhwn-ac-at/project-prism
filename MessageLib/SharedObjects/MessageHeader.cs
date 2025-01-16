namespace MessageLib.SharedObjects
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonObject(MemberSerialization.OptIn)]
    public class MessageHeader(MessageType type)
    {
        private readonly DateTime timestamp = DateTime.Now;
        private readonly MessageType type = type;

        [JsonConstructor]
        public MessageHeader(MessageType type, DateTime timestamp) : this(type)
        {
            this.timestamp=timestamp;
        }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType Type { get => type; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp
        {
            get
            {
                return this.timestamp;
            }
        }
    }
}