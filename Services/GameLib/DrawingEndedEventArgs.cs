namespace GameLib
{
    using System.Collections.Generic;

    public class DrawingEndedEventArgs
    {
        public DrawingEndedEventArgs(int round, Dictionary<string, uint> drawingRoundScore, string searchedWord)
        {
            this.Round=round;
            this.DrawingRoundScore=drawingRoundScore;
            this.SearchedWord=searchedWord;
        }

        public int Round { get; }
        public Dictionary<string, uint> DrawingRoundScore { get; }

        public string SearchedWord { get; }
    }
}