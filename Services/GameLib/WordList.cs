namespace GameLib
{
    using FrenziedMarmot.DependencyInjection;
    using MessageLib;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    [Injectable(Lifetime = ServiceLifetime.Singleton)]
    public class WordList
    {
        private readonly WordListItem[] words;

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

        public WordListItem[] Words => this.words;
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

    public class WordListItem(string word, byte difficulty)
    {
        [JsonProperty("word")]
        [Required]
        public string Word { get; init; } = word;

        [JsonProperty("difficulty")]
        [Required]
        [Range(0, 2)]
        public byte Difficulty { get; init; } = difficulty;
    }
}
