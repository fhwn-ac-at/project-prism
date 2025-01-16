namespace AMQPLib
{
    using MessageLib;

    public interface IAMQPQueueManager
    {
        public Task CreateQueueAsync(string name);

        public Task RemoveQueueAsync(string Name);
    }
}