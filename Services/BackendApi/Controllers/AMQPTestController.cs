namespace BackendApi.Controllers
{
    using AMQPLib;
    using Microsoft.AspNetCore.Mvc;
    using System.Text;

    [ApiController]
    [Route("[controller]")]
    public class AMQPTestController : ControllerBase
    {

        private readonly ILogger<AMQPTestController> _logger;
        private readonly AMQPBroker broker;

        public AMQPTestController(ILogger<AMQPTestController> logger, AMQPBroker broker)
        {
            _logger=logger;
            this.broker=broker;
        }

        [HttpPut(Name = "CreateQueue")]
        public async Task<string> CreateQueue(string name) 
        {
            await this.broker.CreateQueueAsync(name);
            return name;
        }

        [HttpDelete(Name = "DeleteQueue")]
        public async Task<string> DeleteQueue(string name)
        {
            await this.broker.RemoveQueueAsync(name);
            return name;
        }

        [HttpPost(Name = "SendMessage")]
        public async Task<string> SendMessage(string queue, string message, uint ttl)
        {
            await this.broker.SendMessageAsync(queue, Encoding.UTF8.GetBytes(message), ttl);
            return $"{queue}: {message}";
        }
    }
}
