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
                new Location { Istasyon = "Yenikapı", Enlem = 41.00545980430312, Boylam = 28.950467720378626 },
                new Location { Istasyon = "Aksaray", Enlem = 41.01221403282411, Boylam = 28.94859650350184 },
                new Location { Istasyon = "Emniyet-Fatih", Enlem = 41.01778801608544, Boylam = 28.939554244523407 },
                new Location { Istasyon = "Topkapı-Ulubatlı", Enlem = 41.02462594829107, Boylam = 28.93020039294526 },
                new Location { Istasyon = "Sağmalcılar", Enlem = 41.04102319809356, Boylam = 28.907376192946977 },
                new Location { Istasyon = "Kocatepe", Enlem = 41.048599677597274, Boylam = 28.895269235276288 },
                new Location { Istasyon = "Terazidere", Enlem = 41.03065374591455, Boylam = 28.897842179453068 },
                new Location { Istasyon = "Davutpaşa-Ytu,", Enlem = 41.02057403471415, Boylam = 28.90035218650716 },
                new Location { Istasyon = "Merter", Enlem = 41.00759301480838, Boylam = 28.89627743527216 },
                new Location { Istasyon = "Zeytinburnu", Enlem = 41.00174217538679, Boylam = 28.889976322674965 },
                new Location { Istasyon = "Bakırköy-İncirli", Enlem = 40.99676672503808, Boylam = 28.875328550613958 },
                new Location { Istasyon = "Bahçelievler", Enlem = 40.99574664587808, Boylam = 28.863612837121018 },
                new Location { Istasyon = "Ataköy-Şirinevler", Enlem = 40.991387821790255, Boylam = 28.845719578603266 },
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

                await Task.Delay(100, stoppingToken);
            }
        }

    }
}
