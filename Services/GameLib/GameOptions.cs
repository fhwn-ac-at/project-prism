namespace GameLib
{
    using FrenziedMarmot.DependencyInjection;

    [InjectableOptions("GameOptions")]
    public class GameOptions
    {
        public uint WordSelectionCount { get; init; }
        public uint SelectionDuration { get; init; } 
        public uint MaxScore { get; init; }
        public uint MinScore { get; init; }
        public double MinFactor { get; init; }
        public double OrderReduction { get; init; } 
        public double OrderFactorStartPoint { get; init; }
        public uint MaxDrawerScoreTimeScore { get; init; }
        public uint MinDrawerScoreTimeScore { get; init; }
        public double AmountGuessedMaxFactor { get; init; }
        public double EasyWordFactor { get; init; }
        public double MidWordFactor { get; init; }
        public double HardWordFactor { get; init; }
        public ushort DrawingEndedDelay { get; init ; }
    }
}