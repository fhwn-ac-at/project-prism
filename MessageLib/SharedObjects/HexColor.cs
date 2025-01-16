namespace MessageLib.SharedObjects
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public partial class HexColor
    {
        private readonly string hexString;

        public HexColor(string hexString)
        {
            if (!HexStringRegex().IsMatch(hexString))
            {
                throw new ArgumentOutOfRangeException(nameof(hexString));
            }

            this.hexString=hexString;
        }

        [JsonProperty("hexString")]
        [Required]
        public string HexString
        {
            get
            {
                return this.hexString;
            }
        }

        [GeneratedRegex("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")]
        private static partial Regex HexStringRegex();
    }
}
