namespace GameLib
{
    public class GuessCloseEventArgs
    {
        public required string Guess { get; init; }

        public required int Distance { get; init; }

        public required string User { get; init; }
    }
}