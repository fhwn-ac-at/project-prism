namespace AMQPLib
{
    using MessageLib;

    public interface IAMQPBroker
    {
        public Task ConnectToQueueAsync(string name, IMessageDistributor messageDistributor);

        public Task SendMessageAsync(string Queue, ReadOnlyMemory<byte> bytes);
    }
}