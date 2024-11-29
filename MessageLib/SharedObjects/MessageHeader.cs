﻿namespace MessageLib.SharedObjects
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonObject(MemberSerialization.OptIn)]
    public class MessageHeader(MessageType type)
    {
        private readonly DateTime timestamp = DateTime.Now;

        [JsonConstructor]
        public MessageHeader(MessageType type, DateTime timestamp) : this(type)
        {
            this.timestamp = timestamp;
        }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        MessageType Type { get; init; } = type;

        [JsonProperty("timestamp")]
        DateTime Timestamp { get => timestamp; }
    }
}