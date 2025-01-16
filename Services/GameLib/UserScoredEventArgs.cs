namespace GameLib
{
    public class UserScoredEventArgs
    {
        public UserScoredEventArgs(string userId, uint score)
        {
            this.UserId=userId;
            this.Score=score;
        }

        public string UserId { get; }
        public uint Score { get; }
    }
}