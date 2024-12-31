namespace GameLib
{
    using FrenziedMarmot.DependencyInjection;

    [InjectableOptions("GameOptions")]
    public class GameOptions(uint wordSelectionCount, uint selectionDuration, uint maxScore, uint minScore, double minFactor, double orderReduction, double orderFactorStartPoint)
    {
        public uint WordSelectionCount { get; init; } = wordSelectionCount;
        public uint SelectionDuration { get; init; } = selectionDuration;
        public uint MaxScore { get; init; } = maxScore;
        public uint MinScore { get; init; } = minScore;
        public double MinFactor { get; init; } = minFactor;
        public double OrderReduction { get; init; } = orderReduction;
        public double OrderFactorStartPoint { get; init; } = orderFactorStartPoint;
        public uint MaxDrawerScoreTimeScore { get; internal set; }
        public uint MinDrawerScoreTimeScore { get; internal set; }
        public double AmountGuessedMaxFactor { get; internal set; }
        public double EasyWordFactor { get; internal set; }
        public double MidWordFactor { get; internal set; }
        public double HardWordFactor { get; internal set; }
    }
}