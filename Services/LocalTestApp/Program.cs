using MessageLib;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var test = new Validator();

        var valid = await test.Validate("""
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
    }
}