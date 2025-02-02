﻿namespace MessageLib
{
    using FrenziedMarmot.DependencyInjection;
    using MessageLib.SharedObjects;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;

    [Injectable(Lifetime = ServiceLifetime.Singleton)]
    public class Validator(Deserializer deserializer, IOptions<ValidatorOptions> validatorOptions, ILogger<Validator>? logger = null, ILoggerFactory? loggerFactory = null)
    {
        private readonly ILogger<Validator>? logger = logger;
        private readonly Deserializer deserializer = deserializer;
        private readonly bool useNewtonsoft = validatorOptions.Value.useNewtonsoftJsonSchemaValidator;

        private readonly Dictionary<string, JSchema> knownSchemas = [];
        private readonly CustomJSchemaResolver resolver = new CustomJSchemaResolver(loggerFactory?.CreateLogger<CustomJSchemaResolver>());

        public bool Validate(string json)
        {
            return this.Validate(json, out MessageType? _);
        }

        public bool ValidateWithCheckTimestamp(string json, out MessageType? messageType, double maxTimeDifference)
        {
            if (!this.Validate(json, out messageType))
            {
                return false;
            }

            var header = this.deserializer.DeserializeHeader(json);

            if (header == null)
            {
                return false;
            }

            var headerDiffMillis = Math.Abs(DateTime.Now.Subtract(header.Timestamp.ToLocalTime()).TotalMilliseconds);

            if (headerDiffMillis<0||headerDiffMillis>maxTimeDifference)
            {
                this.logger?.LogDebug("Message to old! Message: {}", json);
                return false;
            }

            return true;
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

                string? schemaPath = this.GetSchemaPath(messageType.Value);

                if (schemaPath==null)
                {
                    this.logger?.LogInformation("No Schema found for message");
                    return false;
                }

                if (this.useNewtonsoft)
                {
                    JSchema schema;
                    if (this.knownSchemas.TryGetValue(schemaPath, out JSchema? value))
                    {
                        schema=value;
                    }
                    else
                    {
                        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages", schemaPath);
                        using (TextReader file = File.OpenText(filePath))
                        using (JsonTextReader reader = new JsonTextReader(file))
                        {
                            schema = JSchema.Load(reader, this.resolver);
                        }
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


                string nFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages", schemaPath);
                var schemaTask = NJsonSchema.JsonSchema.FromFileAsync(nFilePath);
                schemaTask.Wait();

                var nSchema = schemaTask.Result;

                var nErrors = nSchema.Validate(json);

                if (nErrors!= null && nErrors.Count>0)
                {
                    this.logger?.LogInformation("Message not valid! Message: {}", json);
                    foreach (NJsonSchema.Validation.ValidationError error in nErrors)
                    {
                        this.logger?.LogInformation(message: error.ToString());
                    }
                    return false;
                }

                return true;
            }
            catch (JsonReaderException ex)
            {
                this.logger?.LogError(ex.Message);
            }

            return false;
        }

        private string? GetSchemaPath(MessageType type)
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
                MessageType.gameStarted => "game/gameStarted.schema.json",
                MessageType.guessClose => "game/guessClose.schema.json",
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
