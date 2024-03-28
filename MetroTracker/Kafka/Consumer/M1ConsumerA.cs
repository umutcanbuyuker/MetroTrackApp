using Confluent.Kafka;
using MetroTracker.Models;
using Newtonsoft.Json;

namespace MetroTracker.Kafka.Consumer
{
    public class M1ConsumerA
    {
        public ConsumerConfig config;
        public M1ConsumerA()
        {
            config = new ConsumerConfig
            {
                GroupId = "testtest",
                BootstrapServers = "localhost:29092"
            };

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
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
