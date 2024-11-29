namespace MessageLib.Lobby
{
    using System.Text.Json.Serialization;

    public class RoundDurationChangedMessage(RoundDurationChangedMessageBody messageBody)
    {
        [JsonPropertyName("header")]
        public MessageHeader MessageHeader { get; init; } = new MessageHeader(MessageType.roundDurationChanged, DateTime.Now);

        [JsonPropertyName("body")]
        public IMessageBody MessageBody { get; init; } = messageBody;
    }
}
