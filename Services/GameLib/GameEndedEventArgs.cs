namespace GameLib
{
    using System.Collections.Generic;

    public class GameEndedEventArgs
    {
        public GameEndedEventArgs(Dictionary<string, uint> drawingRoundScore, string searchedWord)
        {
            this.DrawingRoundScore=drawingRoundScore;
            this.SearchedWord=searchedWord;
        }

        public Dictionary<string, uint> DrawingRoundScore { get; }

        public string SearchedWord { get; }
    }
}