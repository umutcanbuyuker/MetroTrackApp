using Confluent.Kafka;
using MetroTracker.Kafka.Consumer;
using Microsoft.AspNetCore.SignalR;

namespace MetroTracker.Hubs
{
    public class ConsumerHub : Hub
    {
        private readonly LocationConsumer _locationConsumer;
        public ConsumerHub(LocationConsumer locationConsumer)
        {
            _locationConsumer = locationConsumer;   
        }
        public async Task ConsumeMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessagesFromProducer()
        {
            await _locationConsumer.StartConsumingAsync();
        }
    }
}
