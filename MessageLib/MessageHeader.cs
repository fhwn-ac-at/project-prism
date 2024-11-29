namespace MessageLib
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonObject(MemberSerialization.OptIn)]
    public class MessageHeader(MessageType type, DateTime timestamp)
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        MessageType Type { get; init; } = type;

        [JsonProperty("timestamp")]
        DateTime Timestamp { get; init; } = timestamp;
    }
}