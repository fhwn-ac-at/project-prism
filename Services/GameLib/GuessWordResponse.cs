namespace GameLib
{
    public class GuessWordResponse
    {
        public required bool Guessed { get; init; }

        public required IEnumerable<string> Users { get; init; }
    }
}