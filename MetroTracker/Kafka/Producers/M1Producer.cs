using Confluent.Kafka;
using MetroTracker.Models;
using Newtonsoft.Json;


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
			while (!stoppingToken.IsCancellationRequested)
			{

				await Task.Delay(5000);

				using StreamReader reader = new("C:\\Users\\can_u\\source\\repos\\MetroTrackApp\\MetroTracker\\Helpers\\m1locations.json");
				var json = reader.ReadToEnd();

				Location m1 = new Location
				{
					Istasyon = "Şirinevler",
					Enlem = 40.991387821790255,
					Boylam = 28.845719578603266
				};

				using (var producer = new ProducerBuilder<Null, string>(config).Build())
				{
					
						string message = $"{m1.Istasyon}: Enlem: {m1.Enlem}, Boylam: {m1.Boylam}";
						await producer.ProduceAsync("testtest", new Message<Null, string> { Value = message });
						await Task.Delay(3000);
				}

			}
		}
	}
}
