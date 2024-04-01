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

        public IActionResult Ybs()
        {
            return View();
        }
    }
}
