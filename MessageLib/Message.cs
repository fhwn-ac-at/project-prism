namespace MessageLib
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public abstract class Message
    {
        [JsonPropertyName("header")]
        [Required]
        public abstract MessageHeader MessageHeader { get; init; }

        [JsonPropertyName("body")]
        [Required]
        public abstract IMessageBody MessageBody { get; init; }
    }
}
