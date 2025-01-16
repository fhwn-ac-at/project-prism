namespace MessageLib
{
    using FrenziedMarmot.DependencyInjection;
    using MessageLib.SharedObjects;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Text.Json.Nodes;

    // just here so I don't have to import Newtonsoft.Json anywhere and could change the implementation

    [Injectable(Lifetime = ServiceLifetime.Transient)]
    public class Deserializer(ILogger<Deserializer>? logger = null)
    {
        private readonly ILogger<Deserializer>? logger = logger;

        public T? DeserializeTo<T>(string? json) where T : class
        {
            return json==null ? null : JsonConvert.DeserializeObject<T>(json);
        }

        public MessageHeader? DeserializeHeader(string? json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            JObject jsonObject;
            try
            {
                jsonObject = JObject.Parse(json);
            }
            catch (JsonReaderException ex)
            {
                this.logger?.LogError(ex.Message);
                return null;
            }

            if (jsonObject==null
                    ||!jsonObject.HasValues
                    ||!jsonObject.ContainsKey("header")
                    ||!jsonObject.GetValue("header")!.HasValues
                    )
            {
                this.logger?.LogInformation("No header found in : {}", jsonObject?.ToString());
                return null;
            }

            return JsonConvert.DeserializeObject<MessageHeader>(jsonObject.GetValue("header")!.ToString());
        }

        public MessageType? CheckMessageType(string? json)
        {
            if (json==null)
            {
                return null;
            }

            try
            {
                return this.CheckMessageType(JObject.Parse(json));
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
