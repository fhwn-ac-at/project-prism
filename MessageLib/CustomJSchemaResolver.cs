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
    }
}
