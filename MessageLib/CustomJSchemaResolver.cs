using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace MessageLib
{
    internal class CustomJSchemaResolver(ILogger<CustomJSchemaResolver>? logger = null) : JSchemaResolver
    {
        private readonly ILogger<CustomJSchemaResolver>? logger = logger;

        public override Stream? GetSchemaResource(ResolveSchemaContext context, SchemaReference reference)
        {
            if (reference.BaseUri?.OriginalString == "../global.definitions.schema.json")
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages/global.definitions.schema.json");
                var reader = new StreamReader(path);
                return reader.BaseStream;
            }

            this.logger?.LogWarning("Schema not known to CustomJSchemaResolver");
            return null;
        }

        public override JSchema? GetSubschema(SchemaReference reference, JSchema rootSchema)
        {
            if (reference.SubschemaId == null || !reference.SubschemaId.OriginalString.StartsWith("#")) {
                this.logger?.LogWarning("Invalid subschema id");
                return null;
            }

            var pathPices = reference.SubschemaId.OriginalString.Substring(1).Split("/");

            if (pathPices.Length < 1)
            {
                this.logger?.LogWarning("Invalid subschema id");
                return null;
            }

            if (!rootSchema.ExtensionData.TryGetValue(pathPices[0], out JToken? node) || node ==null)
            { 
                this.logger?.LogWarning("Subschema {} not in schema {}", pathPices[0], rootSchema.ToString());
                return null;
            }

            for (int i = 1; i < pathPices.Length; i++)
            {
                var newNode = node.SelectToken(pathPices[i]);
                if (newNode == null)
                {
                    this.logger?.LogWarning("Subschema {} not in schema {}", pathPices[i], node.ToString());
                    return null;
                }
                node = newNode;
            }

            return JSchema.Parse(node.ToString());
        }
    }
}
