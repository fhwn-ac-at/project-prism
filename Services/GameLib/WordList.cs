namespace GameLib
{
    using FrenziedMarmot.DependencyInjection;
    using LoggerLib;
    using MessageLib;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    [Injectable(Lifetime = ServiceLifetime.Singleton)]
    public class WordList
    {
        private readonly WordListItem[] words;

        public WordList(IOptions<WordListOptions> options, Deserializer deserializer, IFileOpener fileOpener)
        {
            if (options == null || options.Value == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.Value.Location==null)
            {
                throw new ArgumentNullException();
            }

            var fileStream = fileOpener.OpenRead(options.Value.Location);

            fileStream.Wait();

            var buffer = new byte[fileStream.Result.Length];
            var readWait = fileStream.Result.ReadAsync(buffer, 0, buffer.Length);

            readWait.Wait();

            var text = System.Text.Encoding.UTF8.GetString(buffer);

            var jsonWords = deserializer.DeserializeTo<JSONWordList>(text);

            if (jsonWords==null || jsonWords.Words==null)
            {
                throw new JsonException();
            }

            this.words=jsonWords.Words;
        }

        public WordListItem[] Words => this.words;
    }

    public class WordListOptions
    {
        public string? Location { get; init; }
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
