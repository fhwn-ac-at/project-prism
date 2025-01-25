namespace GameLib
{
    using FrenziedMarmot.DependencyInjection;

    [InjectableOptions("GameOptions")]
    public class GameOptions
    {
        public required uint WordSelectionCount { get; init; }
        public required uint SelectionDuration { get; init; } 
        public required uint MaxScore { get; init; }
        public required uint MinScore { get; init; }
        public required double MinFactor { get; init; }
        public required double OrderReduction { get; init; } 
        public required double OrderFactorStartPoint { get; init; }
        public required uint MaxDrawerScoreTimeScore { get; init; }
        public required uint MinDrawerScoreTimeScore { get; init; }
        public required double AmountGuessedMaxFactor { get; init; }
        public required double EasyWordFactor { get; init; }
        public required double MidWordFactor { get; init; }
        public required double HardWordFactor { get; init; }
        public required ushort DrawingEndedDelay { get; init ; }
        public required ushort MinUserCount { get; init; }
    }
}