namespace MessageLib
{
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;

    internal class CustomJSchemaResolver(ILogger<CustomJSchemaResolver>? logger = null) : JSchemaResolver
    {
        private readonly ILogger<CustomJSchemaResolver>? logger = logger;

        private string? globalSchema;

        public override Stream? GetSchemaResource(ResolveSchemaContext context, SchemaReference reference)
        {
            if (reference.BaseUri?.OriginalString=="../global.definitions.schema.json")
            {
                if (this.globalSchema==null)
                {
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages/global.definitions.schema.json");
                    this.globalSchema=File.ReadAllText(path);
                }

                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(this.globalSchema);
                writer.Flush();
                stream.Position=0;
                return stream;
            }

            this.logger?.LogWarning("Schema not known to CustomJSchemaResolver");
            return null;
        }

        public override JSchema? GetSubschema(SchemaReference reference, JSchema rootSchema)
        {
            if (reference.SubschemaId==null||!reference.SubschemaId.OriginalString.StartsWith("#"))
            {
                this.logger?.LogWarning("Invalid subschema id");
                return null;
            }

            string[] pathPices = reference.SubschemaId.OriginalString[2..].Split("/");

            if (pathPices.Length<1)
            {
                this.logger?.LogWarning("Invalid subschema id");
                return null;
            }

            if (!rootSchema.ExtensionData.TryGetValue(pathPices[0], out JToken? node)||node==null)
            {
                this.logger?.LogWarning("Subschema {} not in schema {}", pathPices[0], rootSchema.ToString());
                return null;
            }

            for (int i = 1; i<pathPices.Length; i++)
            {
                JToken? newNode = node.SelectToken(pathPices[i]);
                if (newNode==null)
                {
                    this.logger?.LogWarning("Subschema {} not in schema {}", pathPices[i], node.ToString());
                    return null;
                }

                node=newNode;
            }

            return JSchema.Parse(node.ToString());
        }
    }
}
