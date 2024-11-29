using MessageLib.SharedObjects;
using Newtonsoft.Json;

namespace MessageLib.Joined
{
    public class ChatMessageMessageBody(User user, string text) : IMessageBody
    {
        private readonly User user = user;
        private readonly string text = text;

        [JsonProperty("user")]
        public User User { get => user; }

        [JsonProperty("text")]
        public string Text { get => text; }
    }
}