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
        Validator test = new Validator(deserialzier);

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
    }
}