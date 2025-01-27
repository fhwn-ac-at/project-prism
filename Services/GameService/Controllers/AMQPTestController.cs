namespace BackendApi.Controllers
{
    using AMQPLib;
    using MessageLib;
    using Microsoft.AspNetCore.Mvc;
    using System.Text;

    [ApiController]
    [Route("[controller]")]
    public class AMQPTestController : ControllerBase
    {

        private readonly ILogger<AMQPTestController> _logger;
        private readonly IAMQPQueueManager manager;
        private readonly IAMQPBroker broker;
        private readonly IMessageDistributor distributor;

        public AMQPTestController(ILogger<AMQPTestController> logger, IAMQPQueueManager manager, IAMQPBroker broker, IMessageDistributor distributor)
        {
            this._logger=logger;
            this.broker=broker;
            this.manager=manager;
            this.distributor=distributor;
        }

        [HttpPut(Name = "CreateQueue")]
        public async Task<string> CreateQueue(string name)
        {
            await this.manager.CreateQueueAsync(name);
            return name;
        }

        [HttpDelete(Name = "DeleteQueue")]
        public async Task<string> DeleteQueue(string name)
        {
            await this.manager.RemoveQueueAsync(name);
            return name;
        }

        [HttpPost(Name = "SendMessage")]
        public async Task<string> SendMessage(string queue, string message)
        {
            await this.broker.SendMessageAsync(queue, Encoding.UTF8.GetBytes(message));
            return $"{queue}: {message}";
        }

        [HttpGet(Name = "ConnectToQueue")]
        public async Task<string> ConectToQueue(string queue)
        {
            await this.broker.ConnectToQueueAsync(queue, distributor);
            return queue;
        }
    }
}
