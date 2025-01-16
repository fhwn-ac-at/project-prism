namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class UserScoreMessage : Message<UserScoreMessageBody>
    {
        public UserScoreMessage(UserScoreMessageBody body) : base(body, new MessageHeader(MessageType.userScore))
        {

        }

        [JsonConstructor]
        public UserScoreMessage(UserScoreMessageBody body, MessageHeader header) : base(body, header)
        {

        }
    }

    public class UserScoreMessageBody : IMessageBody
    {
        private readonly User user;
        private readonly uint score;

        public UserScoreMessageBody(User user, uint score)
        {
            if (score<0)
            {
                throw new ArgumentOutOfRangeException(nameof(score));
            }

            this.user=user;
            this.score=score;
        }

        [JsonProperty("user")]
        public User User
        {
            get
            {
                return this.user;
            }
        }

        [JsonProperty("score")]
        [Range(0, uint.MaxValue)]
        public uint Score
        {
            get
            {
                return this.score;
            }
        }
    }
}
