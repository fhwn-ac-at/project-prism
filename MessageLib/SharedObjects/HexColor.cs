namespace MessageLib.SharedObjects
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class HexColor(string hexString)
    {
        private readonly string hexString = hexString;

        [JsonProperty("hexString")]
        [Required]
        public string HexString { get =>  hexString; }
    }
}
