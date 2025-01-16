namespace MessageLib.Joined
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;

    public class UserJoinedMessage : Message<UserJoinedMessageBody>
    {
        public UserJoinedMessage(UserJoinedMessageBody body) : base(body, new MessageHeader(MessageType.userJoined))
        { }

        [JsonConstructor]
        public UserJoinedMessage(UserJoinedMessageBody body, MessageHeader header) : base(body, header)
        { }
    }

    public class UserJoinedMessageBody(User user) : IMessageBody
    {
        private readonly User user = user;

        [JsonProperty("user")]
        public User User
        {
            get
            {
                return this.user;
            }
        }
    }
}
