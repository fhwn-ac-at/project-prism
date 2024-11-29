using MessageLib;
using MessageLib.Lobby;

internal class Program
{
    private static void Main(string[] args)
    {
        var test = new Validator();

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
        valid= test.Validate(messageString);
        Console.WriteLine(valid);

        var parsedMessage = Deserializer.DeserializeTo<RoundDurationChangedMessage>(messageString);

        Console.WriteLine();
        Console.WriteLine(parsedMessage);
    }
}