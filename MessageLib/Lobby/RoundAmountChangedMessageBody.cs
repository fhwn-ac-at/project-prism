namespace MessageLib.Lobby
{
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class RoundAmountChangedMessageBody(int rounds) : IMessageBody
    {
        [JsonProperty("rounds")]
        [Range(0, 50)]
        [Required]
        public int Rounds { get; init; } = rounds;
    }
}
