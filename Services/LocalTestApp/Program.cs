using MessageLib;
using MessageLib.Game;
using MessageLib.Lobby;

internal class Program
{
    private static void Main(string[] args)
    {
        var deserialzier = new Deserializer();
        var test = new Validator(deserialzier);

        var message = new RoundDurationChangedMessage(new RoundDurationChangedMessageBody(60));

        var messageString = message.SerializeToJson();

        var valid = test.Validate("""
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

        valid= test.Validate(messageString);
        Console.WriteLine(valid);

        var parsedMessage = deserialzier.DeserializeTo<RoundDurationChangedMessage>(messageString);

        Console.WriteLine();
        Console.WriteLine(parsedMessage);

        Console.WriteLine(deserialzier.CheckMessageType("not json"));

        Console.WriteLine();

        var message2 = new ClearMessage().SerializeToJson();
        Console.WriteLine(message2);
        Console.WriteLine(test.Validate(message2));
        var parsedClear = deserialzier.DeserializeTo<ClearMessage>(message2);
        Console.WriteLine(parsedClear);
    }
}