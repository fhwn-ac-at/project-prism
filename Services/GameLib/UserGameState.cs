namespace GameLib
{
    public class UserGameState
    {
        public UserGameState() : this(0)
        {   
        }

        public UserGameState(uint score)
        {
            this.Score = score;
        }

        public uint Score { get; set; }

        public bool Guessed { get; set; }
    }
}