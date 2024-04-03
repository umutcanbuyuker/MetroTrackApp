using Confluent.Kafka;
using MetroTracker.Controllers;
using MetroTracker.Hubs;
using MetroTracker.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using Newtonsoft.Json;
using System.Text.Json;

namespace MetroTracker.Kafka.Consumer
{
    public class LocationConsumer
    {
        private readonly ConsumerConfig _config;
        private readonly string _topic;
        private readonly IHubContext<ConsumerHub> _hubContext;

        public event EventHandler<string> LocationReceived;

        public LocationConsumer(IHubContext<ConsumerHub> hubContext)
        {
            _config = new ConsumerConfig
            {
                BootstrapServers = "localhost:29092",
                GroupId = "testtest",
            };
            _topic = "testtest";
            _hubContext = hubContext;
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

                        Location? locationNewton = JsonConvert.DeserializeObject<Location>(locationMessage);

                        _hubContext.Clients.All.SendAsync("KafkaMessages", locationNewton);

                        consumer.Close();
                    }
                    catch (ConsumeException ex)
                    {
                        // Handle consume exception
                    }
                }
            });
        }

    }
}
