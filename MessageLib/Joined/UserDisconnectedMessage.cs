namespace MessageLib.Joined
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;

    public class UserDisconnectedMessage : Message<UserDisconnectedMessageBody>
    {
        public UserDisconnectedMessage(UserDisconnectedMessageBody body) : base(body, new MessageHeader(MessageType.userDisconnected))
        {
            
        }

        [JsonConstructor]
        public UserDisconnectedMessage(UserDisconnectedMessageBody body, MessageHeader header) : base(body, header)
        {
        }
    }

    public class UserDisconnectedMessageBody(User user) : IMessageBody
    {
        private readonly User user = user;

        [JsonProperty("user")]
        public User User { get => user; }
    }
}
