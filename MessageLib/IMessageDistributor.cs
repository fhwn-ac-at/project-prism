namespace MessageLib
{
    public interface IMessageDistributor
    {
        public bool HandleMessage(string message);
    }
}