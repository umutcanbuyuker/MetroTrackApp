using Confluent.Kafka;

namespace MetroTrackApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "host1:9092",
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {

            }
        }
    }
}
