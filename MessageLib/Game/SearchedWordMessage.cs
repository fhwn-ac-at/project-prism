namespace MessageLib.Game
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Newtonsoft.Json;
    using System;

    public class SearchedWordMessage : Message<SearchedWordMessageBody>
    {
        public SearchedWordMessage(SearchedWordMessageBody body) : base(body, new MessageHeader(MessageType.searchedWord))
        {
            
        }


        [JsonConstructor]
        public SearchedWordMessage(SearchedWordMessageBody body, MessageHeader header) : base(body, header)
        {
        }
    }

    public class SearchedWordMessageBody(string word) : IMessageBody
    {
        private readonly string word = word;

        [JsonProperty("word")]
        public string Word { get => word; }
    }
}
