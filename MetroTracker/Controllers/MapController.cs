using Confluent.Kafka;
using MetroTracker.Hubs;
using MetroTracker.Kafka.Consumer;
using MetroTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace MetroTracker.Controllers
{
    public class MapController : Controller
    {
        public ConsumerConfig config;
        private readonly LocationConsumer _LocationConsumer;
        private IHubContext<ConsumerHub> _hubContext;
        public MapController(LocationConsumer LocationConsumer)
        {
            config = new ConsumerConfig
            {
                GroupId = "testtest",
                BootstrapServers = "localhost:29092"
            };
            _LocationConsumer = LocationConsumer;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ConsumerA()
        {
            using var consumer = new ConsumerBuilder<Null, string>(config).Build();

            consumer.Subscribe("testtest");

            CancellationTokenSource token = new();

            try
            {
                while (true)
                {
                    var response = consumer.Consume(token.Token);
                    if (response.Message == null)
                    {
                        var location = JsonConvert.DeserializeObject<Location>
                            (response.Message.Value);

                        Location model = new Location { Istasyon = location.Istasyon, Boylam = location.Boylam, Enlem = location.Enlem };
                        ViewData["location"] = model;
                        return View();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IActionResult> LocationPage()
        {
            await _LocationConsumer.StartConsumingAsync();
            
            // Diğer işlemleri gerçekleştir
            return View();
        }


    }
}
