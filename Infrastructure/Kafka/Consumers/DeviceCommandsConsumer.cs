using Confluent.Kafka;
using DevicesManagement.Application;
using DevicesManagement.Domain.Entities;
using Newtonsoft.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace DevicesManagement.Infrastructure.Kafka.Consumers
{
    public class DeviceCommandsConsumer : BackgroundService
    {
        private readonly string topic;
        private IConsumer<string, string> _kafkaConsumer;
        //private readonly DeviceService _deviceService;
        private readonly IServiceProvider _serviceProvider;
        private readonly int _retryConnect;
        private readonly ConsumerConfig _consumerConfig;

        public DeviceCommandsConsumer(IConfiguration config, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _consumerConfig = new ConsumerConfig();
            config.GetSection("Kafka:ConsumerSettings").Bind(_consumerConfig);
            this.topic = config.GetValue<string>("Kafka:DeviceCommandsTopic");
            _kafkaConsumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();


            _retryConnect = config.GetValue<int>("Kafka:RetryConnect");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        }

        private async Task StartConsumerLoop(CancellationToken cancellationToken)
        {
            _kafkaConsumer.Subscribe(this.topic);
            var result = _kafkaConsumer.Consume(TimeSpan.FromMilliseconds(100));



            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = _kafkaConsumer.Consume(cancellationToken);

                    var device = JsonConvert.DeserializeObject<DeviceDTO>(cr.Message.Value);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var _deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();
                        await _deviceService.AddOrUpdateDevice(device);
                    }                        
                    // Handle message...
                    //Console.WriteLine($"{cr.Message.Key}: {cr.Message.Value}ms");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    // Consumer errors should generally be ignored (or logged) unless fatal.
                    Console.WriteLine($"Consume error: {e.Error.Reason}");

                    if (e.Error.IsFatal)
                    {
                        // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e}");
                    break;
                }
            }
        }

        public override void Dispose()
        {
            _kafkaConsumer.Close(); // Commit offsets and leave the group cleanly.
            _kafkaConsumer.Dispose();

            base.Dispose();
        }
    }
}
