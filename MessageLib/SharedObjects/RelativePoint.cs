namespace MessageLib.SharedObjects
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class RelativePoint
    {
        private readonly double x;
        private readonly double y;

        public RelativePoint(double x, double y)
        {
            if (x<0||x>100)
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (y<0||y>100)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            this.x=x;
            this.y=y;
        }

        [JsonProperty("x")]
        [Range(0, 100)]
        [Required]
        public double X
        {
            get
            {
                return this.x;
            }
        }

        [JsonProperty("y")]
        [Range(0, 100)]
        [Required]
        public double Y
        {
            get
            {
                return this.y;
            }
        }
    }
}
