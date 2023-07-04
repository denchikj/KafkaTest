using KafkaFlow;
using KafkaStreams.Shared;
using Microsoft.Extensions.Configuration;

namespace KafkaStreams.Consumer
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = Extensions.GetConfiguration();
            IServiceProvider services = Extensions.GetServices(configuration);

            var bus = services.CreateKafkaBus();
            await bus.StartAsync();

            Console.WriteLine("Consumer started. To stop the consumer press any key.");
            Console.WriteLine($"Ball possession\n\n{Constants.HomeTeam} : {Constants.AwayTeam}");
            Console.ReadKey();

            await bus.StopAsync();
        }
    }
}