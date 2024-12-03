namespace MessageLib
{
    using MessageLib.SharedObjects;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    // just here so I don't have to import Newtonsoft.Json anywhere and could change the implementation
    public class Deserializer(ILogger<Deserializer>? logger = null)
    {
        private readonly ILogger<Deserializer>? logger = logger;

        public T? DeserializeTo<T>(string? json) where T : class
        {
            return json==null ? null : JsonConvert.DeserializeObject<T>(json);
        }

        public MessageType? CheckMessageType(string? json)
        {
            if (json==null)
            {
                return null;
            }

            try
            {
                this.CheckMessageType(JObject.Parse(json));
            }
            catch (JsonReaderException ex)
            {
                this.logger?.LogError(ex.Message);
            }

            return null;
        }

        public MessageType? CheckMessageType(JObject jsonObject)
        {
            if (jsonObject==null
                    ||!jsonObject.HasValues
                    ||!jsonObject.ContainsKey("header")
                    ||!jsonObject.GetValue("header")!.HasValues
                    ||jsonObject.GetValue("header")!.SelectToken("type")==null
                    ||jsonObject!.GetValue("header")!.SelectToken("type")!.Value<string>()==null
                    )
            {
                this.logger?.LogInformation("No valid type found in message: {}", jsonObject?.ToString());
                return null;
            }

            return jsonObject!.GetValue("header")!.SelectToken("type")!.ToObject<MessageType>();
        }
    }
}
