namespace MessageLib
{
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;
    using System.Text.Json.Nodes;

    public class Validator
    {
        public async Task<bool> Validate(string json)
        {
            if (string.IsNullOrEmpty(json)) return false;

            var jsonObject = JObject.Parse(json);
            //var jsonNode = JsonNode.Parse(json);

            if (jsonObject==null 
                || !jsonObject.HasValues 
                || !jsonObject.ContainsKey("header") 
                || !jsonObject.GetValue("header")!.HasValues 
                || jsonObject.GetValue("header")!.SelectToken("type") == null
                )
            {
                return false;
            }

            var type = jsonObject.GetValue("header")?.SelectToken("type")!.Value<string>();

            var filePath = type switch
            {
                nameof(MessageType.roundDurationChanged) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages/lobby/roundDurationChanged.schema.json"),
                _ => null
            };

            if (filePath == null)
            {
                return false;
            }

            // TODO develop own resolver
            var resolver = new JSchemaPreloadedResolver();

            // Add the meta-schema to the resolver.
            resolver.Add(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages/global.definitions.schema.json")), File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages/global.definitions.schema.json")));

            //var reader = new StreamReader(filePath);
            //var jsonStr = reader.ReadToEnd();
            var schema = JSchema.Parse(File.ReadAllText(filePath), resolver);

            //var schema = await JsonSchema.FromJsonAsync(jsonStr);
            //var validator = new JsonSchemaValidator();
            var result = jsonObject.IsValid(schema);

            return result;
        }
    }    
}
