namespace MessageLib.Lobby
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class RoundDurationChangedMessageBody(int duration) : IMessageBody
    {
        [JsonPropertyName("duration")]
        [Range(0, 500)]
        [Required]
        public int Duration { get; init; } = duration;
    }
}
