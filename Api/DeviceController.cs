using Confluent.Kafka;
using DevicesManagement.Domain.Entities;
using DevicesManagement.Infrastructure.Kafka.Producers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DevicesManagement.Api
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {
        private string topic;
        private readonly DeviceStatusesProducer<Null, string> producer;
        private readonly ILogger logger;

        public DeviceController(DeviceStatusesProducer<Null, string> producer, IConfiguration config, ILogger<DeviceController> logger)
        {
            topic = config.GetValue<string>("Kafka:DeviceStatuses");
            this.producer = producer;
            this.logger = logger;
        }

        [HttpGet]

        public ContentResult Index()
        {
            return Content("ok!");
        }


        [HttpPost]
        public async Task<JsonResult> Create(DeviceDTO deviceStatus)
        {
            await producer.ProduceAsync(topic, new Message<Null, string> { Value = JsonConvert.SerializeObject(deviceStatus) });

            return new JsonResult("Ok");
        }

    }
}
