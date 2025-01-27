using GameLib;
using LoggerLib;
using MessageLib;
using MessageLib.Game;
using MessageLib.Lobby;
using MessageLib.SharedObjects;
using Microsoft.Extensions.Options;

internal class Program
{
    private static void Main(string[] args)
    {
        Deserializer deserialzier = new Deserializer();
        Validator test = new Validator(deserialzier, Options.Create(new ValidatorOptions { useNewtonsoftJsonSchemaValidator=false}));

        RoundDurationChangedMessage message = new RoundDurationChangedMessage(new RoundDurationChangedMessageBody(60));

        string messageString = message.SerializeToJson();

        bool valid = test.Validate("""
        {
            header: {
                type: "roundDurationChanged",
                timestamp: "2024-11-28T19:35:43Z"
            },
            body: {
                duration: 60
            }
        }
""");

        Console.WriteLine(valid);

        Console.WriteLine();
        Console.WriteLine(messageString);
        Console.WriteLine("Has type");
        Console.WriteLine(deserialzier.CheckMessageType(messageString));

        valid=test.Validate(messageString);
        Console.WriteLine(valid);

        RoundDurationChangedMessage? parsedMessage = deserialzier.DeserializeTo<RoundDurationChangedMessage>(messageString);

        Console.WriteLine();
        Console.WriteLine(parsedMessage);

        Console.WriteLine(deserialzier.CheckMessageType("not json"));

        Console.WriteLine();

        string message2 = new ClearMessage().SerializeToJson();
        Console.WriteLine(message2);
        Console.WriteLine(test.Validate(message2));
        ClearMessage? parsedClear = deserialzier.DeserializeTo<ClearMessage>(message2);
        Console.WriteLine(parsedClear);

        Console.WriteLine();
        string message3 = new SetDrawerMessage(new SetDrawerMessageBody([new SelectWordItem("0", 0), new SelectWordItem("1", 1), new SelectWordItem("2", 2)])).SerializeToJson();
        Console.WriteLine(message3);
        Console.WriteLine(test.Validate(message3));
        SetDrawerMessage? parsedSetDrwaer = deserialzier.DeserializeTo<SetDrawerMessage>(message3);
        Console.WriteLine(parsedSetDrwaer);

        WordList list = new WordList(Options.Create(new WordListOptions() { Location ="./wordlist.json" }), deserialzier, new FileOpener(Options.Create(new FileOpenerOptions()), null));

        Console.WriteLine(list);

        var longestWord = list.Words.Max((item) => item.Word.Length);
        var splitWords = list.Words.Any((item) => item.Word.Contains(' '));
        var dashedWords = list.Words.Any((item) => item.Word.Contains('-'));

        Console.WriteLine(longestWord);
        Console.WriteLine(splitWords);
        Console.WriteLine(dashedWords);

        NextRoundMessage nextRoundMessage = new NextRoundMessage(new NextRoundMessageBody("test", 1, new Dictionary<string, uint>
        {
            { "key1", 1 },
            { "key2", 2 }
        }));

        Console.WriteLine(nextRoundMessage.SerializeToJson());
        Console.WriteLine(test.Validate(nextRoundMessage.SerializeToJson(), out MessageType? nextRoundMessageType));
        Console.WriteLine(nextRoundMessageType);

        string testNextRoundMessage = "{\"header\":{\"type\":\"nextRound\",\"timestamp\":\"2025-01-26T14:16:33.4445654+01:00\"},\"body\":{\"word\":\"test\",\"round\":1,\"score\":{1:1,\"key2\":2}}}";

        Console.WriteLine(test.Validate(testNextRoundMessage, out MessageType? testNextRoundMessageType));
        Console.WriteLine(testNextRoundMessageType);
        var TestNextRoundMessageDe = deserialzier.DeserializeTo<NextRoundMessage>(testNextRoundMessage);
        Console.WriteLine(TestNextRoundMessageDe);

        GameEndedMessage gameEndedMessage = new GameEndedMessage(new GameEndedMessageBody("test", new Dictionary<string, uint>
        {
            { "key1", 1 },
            { "key2", 2 }
        }));

        Console.WriteLine(gameEndedMessage.SerializeToJson());
        Console.WriteLine(test.Validate(gameEndedMessage.SerializeToJson(), out MessageType? gameEndedMessageType));
        Console.WriteLine(gameEndedMessageType);

        var TestGameEndedMessageDe = deserialzier.DeserializeTo<NextRoundMessage>(gameEndedMessage.SerializeToJson());
        Console.WriteLine(TestGameEndedMessageDe);

        GuessCloseMessage guessCloseMessage = new GuessCloseMessage(new GuessCloseMessageBody("test", 2));

        Console.WriteLine(guessCloseMessage.SerializeToJson());
        Console.WriteLine(test.Validate(guessCloseMessage.SerializeToJson(), out MessageType? guessCloseMessageType));
        Console.WriteLine(guessCloseMessageType);

        var guessCloseMessageDe = deserialzier.DeserializeTo<GuessCloseMessage>(guessCloseMessage.SerializeToJson());
        Console.WriteLine(guessCloseMessageDe);
    }
}