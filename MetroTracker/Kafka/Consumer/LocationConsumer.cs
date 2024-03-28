using Confluent.Kafka;
using MetroTracker.Controllers;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;

namespace MetroTracker.Kafka.Consumer
{
    public class LocationConsumer
    {
        private readonly ConsumerConfig _config;
        private readonly string _topic;
        public event EventHandler<string> LocationReceived;

        public LocationConsumer(IConfiguration configuration)
        {
            _config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "testtest",
            };
            _topic = "testtest";
        }

        public async Task StartConsumingAsync()
        {
            await Task.Run(() =>
            {
                using var consumer = new ConsumerBuilder<Null, string>(_config).Build();
                consumer.Subscribe(_topic);

                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(CancellationToken.None);
                        var locationMessage = consumeResult.Message.Value;

                        OnLocationReceived(locationMessage);
                    }
                    catch (ConsumeException ex)
                    {
                        // Handle consume exception
                    }
                }
            });
        }

        protected virtual void OnLocationReceived(string locationMessage)
        {
            LocationReceived?.Invoke(this, locationMessage);
        }
    }
}
