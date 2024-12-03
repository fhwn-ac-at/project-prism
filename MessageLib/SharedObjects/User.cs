namespace MessageLib.SharedObjects
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class User(string name, string id)
    {
        private readonly string name = name;
        private readonly string id = id;

        [JsonProperty("name")]
        [Required]
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        [JsonProperty("id")]
        [Required]
        public string Id
        {
            get
            {
                return this.id;
            }
        }
    }
}
