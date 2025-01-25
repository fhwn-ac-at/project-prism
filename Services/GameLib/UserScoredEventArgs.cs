namespace GameLib
{
    public class UserScoredEventArgs
    {
        public UserScoredEventArgs(string userId, uint score, string searchedWord)
        {
            this.UserId=userId;
            this.Score=score;
            this.SearchedWord=searchedWord;
        }

        public string UserId { get; }
        public uint Score { get; }

        public string SearchedWord { get; }
    }
}