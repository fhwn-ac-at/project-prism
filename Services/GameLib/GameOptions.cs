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
        public uint MaxDrawerScoreTimeScore { get; internal set; }
        public uint MinDrawerScoreTimeScore { get; internal set; }
        public double AmountGuessedMaxFactor { get; internal set; }
        public double EasyWordFactor { get; internal set; }
        public double MidWordFactor { get; internal set; }
        public double HardWordFactor { get; internal set; }
    }
}