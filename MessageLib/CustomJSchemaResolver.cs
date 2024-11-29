using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace MessageLib
{
    public class CustomJSchemaResolver : JSchemaResolver
    {
        public override Stream? GetSchemaResource(ResolveSchemaContext context, SchemaReference reference)
        {
            if (reference.BaseUri?.OriginalString == "../global.definitions.schema.json")
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages/global.definitions.schema.json");
                var reader = new StreamReader(path);
                return reader.BaseStream;
            }

            return null;
        }

        public override JSchema? GetSubschema(SchemaReference reference, JSchema rootSchema)
        {
            if (reference.SubschemaId == null || !reference.SubschemaId.OriginalString.StartsWith("#")) {
                return null;
            }

            var pathPices = reference.SubschemaId.OriginalString.Substring(1).Split("/");

            if (pathPices.Length < 1)
            {
                return null;
            }

            if (!rootSchema.ExtensionData.TryGetValue(pathPices[0], out JToken? node) || node == null)
            {
                return null;
            }

            for (int i = 1; i < pathPices.Length; i++)
            {
                var newNode = node.SelectToken(pathPices[i]);
                if (newNode == null)
                {
                    return null;
                }
                node = newNode;
            }

            return JSchema.Parse(node.ToString());
        }

        public override SchemaReference ResolveSchemaReference(ResolveSchemaContext context)
        {
            return base.ResolveSchemaReference(context);
        }
    }
}
