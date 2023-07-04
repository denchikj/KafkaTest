using Microsoft.Extensions.Configuration;
using Streamiz.Kafka.Net;

namespace KafkaStreams.Processor
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = Extensions.GetConfiguration();
            IServiceProvider services = Extensions.GetServices(configuration);

            var streamConfig = Extensions.GetStreamConfig(configuration);
            var topology = TopologyBuilder.Build();

            var stream = new KafkaStream(topology, streamConfig);
            await stream.StartAsync();

            Console.WriteLine("Processor started. To stop the processor press Ctrl+C.");
        }
    }
}