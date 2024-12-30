namespace GameLib
{
    using MessageLib;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class WordList
    {
        private WordListItem[] words;

        public WordList(IOptions<WordListOptions> options, Deserializer deserializer)
        {
            if (options == null || options.Value == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.Value.Location==null)
            {
                throw new ArgumentNullException();
            }

            var text = File.ReadAllTextAsync(options.Value.Location);

            text.Wait();

            var jsonWords = deserializer.DeserializeTo<JSONWordList>(text.Result);

            if (jsonWords==null)
            {
                throw new ArgumentException();
            }

            this.words=jsonWords.Words;
        }

    }

    public class WordListOptions(string location)
    {
        public string Location { get; init; } = location;
    }

    internal class JSONWordList(WordListItem[] words)
    {
        [JsonProperty("list")]
        [Required]
        public WordListItem[] Words { get; init; } = words;
    }

    internal class WordListItem(string word, byte difficulty)
    {
        [JsonProperty("word")]
        [Required]
        public string Word { get; init; } = word;

        [JsonProperty("difficulty")]
        [Required]
        public byte Difficulty { get; init; } = difficulty;
    }
}
