namespace MessageLib
{
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;

    public class Validator
    {
        public bool Validate(string json)
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
            var resolver = new CustomJSchemaResolver();

            var metaSchema = JSchema.Parse(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages/global.definitions.schema.json")), resolver);
            var schema = JSchema.Parse(File.ReadAllText(filePath), resolver);

            IList<ValidationError> errors;
            bool valid = jsonObject.IsValid(schema, out errors);

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    Console.WriteLine(error.Message);
                }
            }

            return valid;
        }
    }    
}
