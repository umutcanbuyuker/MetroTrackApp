using Confluent.Kafka;
using MetroTracker.Models;
using System.Text.Json;


namespace MetroTracker.Kafka.Producers
{
	public class M1Producer : BackgroundService
	{
		public ProducerConfig config;

		public M1Producer()
		{
			config = new ProducerConfig
			{
				BootstrapServers = "localhost:29092"
            };
		}
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Konumları tanımla
            List<Location> locations = new List<Location>
            {
                new Location { Istasyon = "Sirinevler", Enlem = 40.991387821790255, Boylam = 28.845719578603266 },
                new Location { Istasyon = "Ara1", Enlem = 40.99147561134383, Boylam = 28.840728625322384 },
                new Location { Istasyon = "Yenibosna", Enlem = 40.98943356672049, Boylam = 28.83685234017511 },
                new Location { Istasyon = "Ara2", Enlem = 40.987921202137834, Boylam = 28.83257843357325 },
                new Location { Istasyon = "Fuar Merkezi", Enlem = 40.986591482483, Boylam = 28.828568763176964 },
                new Location { Istasyon = "Ara3", Enlem = 40.98464579947127, Boylam = 28.822564927603914 },
                new Location { Istasyon = "Ataturk Havalimanı", Enlem = 40.97951286972792, Boylam = 28.821109311338773 }
            };

            int currentIndex = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                Location currentLocation = locations[currentIndex];

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                string jsonMessage = JsonSerializer.Serialize(currentLocation, options);

                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    await producer.ProduceAsync("testtest", new Message<Null, string> { Value = jsonMessage });
                }

                currentIndex = (currentIndex + 1) % locations.Count;

                await Task.Delay(30000, stoppingToken);
            }
        }

    }
}
