using GameLib;
using LoggerLib;
using MessageLib;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;

namespace UnitTests.GameLib
{
    [TestFixture]
    public class WordListTest
    {
        // needed because of weird c# generics
        private Stream GetMemoryStream(string text)
        {
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text));
        }

        [Test]
        public void ParseWordList()
        {
            var wordListOptions = new WordListOptions()
            {
                Location="Test"
            };

            var mockedFileOpener = new Mock<IFileOpener>();
            mockedFileOpener.Setup(x => x.OpenRead("Test")).Returns(Task.Run(() => this.GetMemoryStream("""{"list": [{"word": "test", "difficulty":0},{"word": "test1", "difficulty":1},{"word": "test2", "difficulty":2}]}""")));

            var wordlist = new WordList(Options.Create(wordListOptions), new Deserializer(), mockedFileOpener.Object);

            Assert.That(wordlist.Words.Length, Is.EqualTo(3));
            Assert.That(wordlist.Words[0].Word, Is.EqualTo("test"));
            Assert.That(wordlist.Words[0].Difficulty, Is.EqualTo(0));
            Assert.That(wordlist.Words[1].Word, Is.EqualTo("test1"));
            Assert.That(wordlist.Words[1].Difficulty, Is.EqualTo(1));
            Assert.That(wordlist.Words[2].Word, Is.EqualTo("test2"));
            Assert.That(wordlist.Words[2].Difficulty, Is.EqualTo(2));
        }

        [Test]
        public void WordListOptionLocationNull()
        {
            Assert.Throws<ArgumentNullException>(() => { new WordList(Options.Create(new WordListOptions()), new Deserializer(), new FileOpener(Options.Create(new FileOpenerOptions()), null)); });
        }

        [Test]
        public void WordListNoOptions()
        {
            Assert.Throws<ArgumentNullException>(() => { new WordList(null!, new Deserializer(), new FileOpener(Options.Create(new FileOpenerOptions()), null)); });
        }

        [Test]
        public void ParseWordListInvalidJson()
        {
            var wordListOptions = new WordListOptions()
            {
                Location="Test"
            };

            var mockedFileOpener = new Mock<IFileOpener>();
            mockedFileOpener.Setup(x => x.OpenRead("Test")).Returns(Task.Run(() => this.GetMemoryStream("""{"test":[]}""")));

            Assert.Throws<JsonException>(() => { new WordList(Options.Create(wordListOptions), new Deserializer(), mockedFileOpener.Object); });
        }
    }
}
