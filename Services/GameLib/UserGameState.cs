namespace GameLib
{
    public class UserGameState
    {
        public UserGameState() : this(0)
        {   
        }

        public UserGameState(int score)
        {
            this.Score = score;
        }

        public int Score { get; set; }

        public bool Guessed { get; set; }
    }
}