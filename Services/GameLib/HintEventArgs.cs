namespace GameLib
{
    public class HintEventArgs
    {

        public required string Hint { get; init; }

        public required IEnumerable<string> Users { get; init; }
    }
}