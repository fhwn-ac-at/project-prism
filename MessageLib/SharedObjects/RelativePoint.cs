namespace MessageLib.SharedObjects
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class RelativePoint(double x, double y)
    {
        private readonly double x = x; 
        private readonly double y = y;

        [JsonProperty("x")]
        [Range(0, 100)]
        [Required]
        public double X { get => x; }

        [JsonProperty("y")]
        [Range(0, 100)]
        [Required]
        public double Y { get => y; }
    }
}
