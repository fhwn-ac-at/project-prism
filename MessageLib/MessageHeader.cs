namespace MessageLib
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class MessageHeader(MessageType type, DateTime timestamp)
    {
        [JsonPropertyName("type")]
        [Required]
        MessageType Type { get; init; } = type;

        [JsonPropertyName("timestamp")]
        [Required]
        DateTime Timestamp { get; init; } = timestamp;
    }
}