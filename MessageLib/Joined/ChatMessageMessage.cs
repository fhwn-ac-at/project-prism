namespace MessageLib.Joined
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;

    public class ChatMessageMessage : Message<ChatMessageMessageBody>
    {
        public ChatMessageMessage(ChatMessageMessageBody body) : base(body, new MessageHeader(MessageType.chatMessage))
        {

        }

        [JsonConstructor]
        public ChatMessageMessage(ChatMessageMessageBody body, MessageHeader header) : base(body, header)
        {
        }
    }

    public class ChatMessageMessageBody(User user, string text) : IMessageBody
    {
        private readonly User user = user;
        private readonly string text = text;

        [JsonProperty("user")]
        public User User
        {
            get
            {
                return this.user;
            }
        }

        [JsonProperty("text")]
        public string Text
        {
            get
            {
                return this.text;
            }
        }
    }
}
