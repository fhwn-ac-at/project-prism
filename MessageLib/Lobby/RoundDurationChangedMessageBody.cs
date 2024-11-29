namespace MessageLib.Lobby
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class RoundDurationChangedMessageBody(int duration) : IMessageBody
    {
        [JsonProperty("duration")]
        [Range(0, 500)]
        [Required]
        public int Duration { get; init; } = duration;
    }
}
