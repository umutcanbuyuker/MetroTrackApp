
using Confluent.Kafka;
using MetroTracker.Models;
using System.Text.Json;

namespace MetroTracker.Kafka.Producers
{
    public class M2Producer : BackgroundService
    {
        public ProducerConfig config;
        public M2Producer()
        {
            config = new ProducerConfig
            {
                BootstrapServers = "localhost:29092"
            };
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            List<Location> locations = new List<Location>
        {
            new Location { Istasyon = "Yenikapı", Enlem = 41.00545980430312, Boylam = 28.950467720378626 },
            new Location { Istasyon = "Vezneciler-İstanbul Ü. İstasyonu", Enlem = 41.03362283244409, Boylam = 28.95636698549313 },
            new Location { Istasyon = "Haliç İstasyonu", Enlem = 41.04636814829871, Boylam = 28.94782629011491 },
            new Location { Istasyon = "Şişhane İstasyonu", Enlem = 41.02872349326307, Boylam = 28.970829695497372 },
            new Location { Istasyon = "Taksim İstasyonu", Enlem = 41.03678968370631, Boylam = 28.985032289055122 },
            new Location { Istasyon = "Osmanbey İstasyonu", Enlem = 41.043900873939726, Boylam = 28.985032289055122 },
            new Location { Istasyon = "Şişli-Mecidiyeköy İstasyonu", Enlem = 41.06310845441645, Boylam = 28.98843821586349 },
            new Location { Istasyon = "Gayrettepe İstasyonu", Enlem = 41.06744952254079, Boylam = 28.989981076096105 },
            new Location { Istasyon = "Levent İstasyonu", Enlem = 41.082310173344025, Boylam = 29.0056664294055 },
            new Location { Istasyon = "4.Levent İstasyonu", Enlem = 41.09895222204967, Boylam = 29.012885599315675 },
            new Location { Istasyon = "Sanayi Mahallesi İstasyonu", Enlem = 41.09932025868841, Boylam = 29.02546247196075 },
            new Location { Istasyon = "Seyrantepe İstasyonu", Enlem = 41.10219404138266, Boylam = 29.011872794826796 },
            new Location { Istasyon = "İYÜ-Ayazağa İstasyonu", Enlem = 41.10598694710771, Boylam = 29.023207300746887 },
            new Location { Istasyon = "Atatürk Oto Sanayi İstasyonu", Enlem = 41.05356984020996, Boylam = 28.854609320711833 },
            new Location { Istasyon = "Darüşafaka İstasyonu", Enlem = 41.092491126252014, Boylam = 29.003726711350686 },
            new Location { Istasyon = "Hacıosman İstasyonu", Enlem = 41.10756673128821, Boylam = 29.034979883708025 }
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
                    await producer.ProduceAsync("M2Location", new Message<Null, string> { Value = jsonMessage });
                }

                currentIndex = (currentIndex + 1) % locations.Count;

                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
