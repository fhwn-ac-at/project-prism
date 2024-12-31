namespace GameLib
{
    using System.Collections.Generic;

    public class DrawingEndedEventArgs
    {
        public DrawingEndedEventArgs(int round, Dictionary<string, uint> drawingRoundScore)
        {
            this.Round=round;
            this.DrawingRoundScore=drawingRoundScore;
        }

        public int Round { get; }
        public Dictionary<string, uint> DrawingRoundScore { get; }
    }
}