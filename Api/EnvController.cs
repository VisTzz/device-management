using Confluent.Kafka;
using DevicesManagement.Domain.Entities;
using DevicesManagement.Infrastructure.Kafka.Producers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DevicesManagement.Api
{
    [ApiController]
    [Route("[controller]")]
    public class EnvController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IConfiguration config;

        public EnvController(IConfiguration config, ILogger<DeviceController> logger)
        {
            this.logger = logger;
            this.config = config;
        }

        [HttpGet]

        public ContentResult Index()
        {
            var res = "Postgress: " + config.GetValue<string>("ConnectionStrings:DefaultConnection");
            res += "\nKafka: " + config.GetValue<string>("Kafka:ProducerSettings:BootstrapServers");

            return Content(res);
        }

    }
}
