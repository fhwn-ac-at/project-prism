namespace BackendApi.Controllers
{
    using BackendApi.AMQP;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Text;

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly AMQPBroker broker;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AMQPBroker broker)
        {
            _logger=logger;
            this.broker = broker;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogInformation(this.User.ExtractDisplayName());

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date=DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC=Random.Shared.Next(-20, 55),
                Summary=Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
