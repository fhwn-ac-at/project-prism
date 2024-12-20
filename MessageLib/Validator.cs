namespace MessageLib
{
    using FrenziedMarmot.DependencyInjection;
    using MessageLib.SharedObjects;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;

    [Injectable(Lifetime = ServiceLifetime.Singleton)]
    public class Validator(Deserializer deserializer, ILogger<Validator>? logger = null, ILoggerFactory? loggerFactory = null)
    {
        private readonly ILogger<Validator>? logger = logger;
        private readonly ILoggerFactory? loggerFactory = loggerFactory;
        private readonly Deserializer deserializer = deserializer;

        private readonly Dictionary<string, JSchema> knownSchemas = [];
        private readonly CustomJSchemaResolver resolver = new CustomJSchemaResolver(loggerFactory?.CreateLogger<CustomJSchemaResolver>());

        public bool Validate(string json)
        {
            return this.Validate(json, out MessageType? _);
        }

        public bool Validate(string json, out MessageType? messageType)
        {
            messageType=null;
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            try
            {
                JObject jsonObject = JObject.Parse(json);

                messageType=this.deserializer.CheckMessageType(jsonObject);

                if (!messageType.HasValue)
                {
                    return false;
                }

                string? schemaPath = this.getSchemaPath(messageType.Value);

                if (schemaPath==null)
                {
                    this.logger?.LogInformation("No Schema found for message");
                    return false;
                }

                JSchema schema;
                if (this.knownSchemas.TryGetValue(schemaPath, out JSchema? value))
                {
                    schema=value;
                }
                else
                {
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages", schemaPath);
                    schema=JSchema.Parse(File.ReadAllText(filePath), this.resolver);
                }

                bool valid = jsonObject.IsValid(schema, out IList<ValidationError> errors);

                if (errors.Count>0)
                {
                    this.logger?.LogInformation("Message not valid! Message: {}", json);
                    foreach (ValidationError error in errors)
                    {
                        this.logger?.LogInformation(error.Message);
                    }
                }

                return valid;
            }
            catch (JsonReaderException ex)
            {
                this.logger?.LogError(ex.Message);
            }

            return false;
        }

        private string? getSchemaPath(MessageType type)
        {
            return type switch
            {
                // lobby messages
                MessageType.roundDurationChanged => "lobby/roundDurationChanged.schema.json",
                MessageType.roundAmountChanged => "lobby/roundAmountChanged.schema.json",

                // joined messages
                MessageType.chatMessage => "joined/chatMessage.schema.json",
                MessageType.userDisconnected => "joined/userDisconnected.schema.json",
                MessageType.userJoined => "joined/userJoined.schema.json",

                // game messages
                MessageType.backgroundColor => "game/backgroundColor.schema.json",
                MessageType.clear => "game/clear.schema.json",
                MessageType.closePath => "game/closePath.schema.json",
                MessageType.drawingSizeChanged => "game/drawingSizeChanged.schema.json",
                MessageType.gameEnded => "game/gameEnded.schema.json",
                MessageType.lineTo => "game/lineTo.schema.json",
                MessageType.moveTo => "game/moveTo.schema.json",
                MessageType.nextRound => "game/nextRound.schema.json",
                MessageType.point => "game/point.schema.json",
                MessageType.searchedWord => "game/searchedWord.schema.json",
                MessageType.selectWord => "game/selectWord.schema.json",
                MessageType.setDrawer => "game/setDrawer.schema.json",
                MessageType.setNotDrawer => "game/setNotDrawer.schema.json",
                MessageType.undo => "game/undo.schema.json",
                MessageType.userScore => "game/userScore.schema.json",
                _ => null
            };
        }
    }
}
