using KafkaFlow;
using KafkaFlow.Producers;
using KafkaFlow.Serializer;
using KafkaStreams.Producer.Options;
using KafkaStreams.Producer.Services;
using KafkaStreams.Producer.Services.Interfaces;
using KafkaStreams.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace KafkaStreams.Producer
{
    public static class Extensions
    {
        public static IConfiguration GetConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pending>")]
        public static IServiceProvider GetServices(IConfiguration configuration) =>
            new ServiceCollection()
                .AddSingleton(configuration)
                .AddOptions(configuration)
                .AddSystemServices()
                .AddKafka(kafka =>
                {
                    var lineupProducerOpt = configuration.GetSection(nameof(LineupProducerOptions)).Get<LineupProducerOptions>();
                    var passProducerOpt = configuration.GetSection(nameof(PassProducerOptions)).Get<PassProducerOptions>();

                    kafka.AddCluster(cluster => cluster
                        .WithBrokers(new[] { Constants.BootstrapServers })
                        .CreateTopicIfNotExists(Constants.LineupTopic, lineupProducerOpt.NumberOfPartitions, lineupProducerOpt.ReplicationFactor)
                        .AddProducer(lineupProducerOpt.Name, producer => producer
                            .DefaultTopic(Constants.LineupTopic)
                            .AddMiddlewares(x => x.AddSerializer<JsonCoreSerializer>()))
                        .CreateTopicIfNotExists(Constants.PassesTopic, passProducerOpt.NumberOfPartitions, passProducerOpt.ReplicationFactor)
                        .AddProducer(passProducerOpt.Name, producer => producer
                            .DefaultTopic(Constants.PassesTopic)
                            .AddMiddlewares(x => x.AddSerializer<JsonCoreSerializer>())));
                })
                .BuildServiceProvider();

        private static IMessageProducer GetProducer<T>(IServiceProvider services) where T : ProducerOptions
        {
            var options = services.GetRequiredService<IOptions<T>>();

            return services.GetRequiredService<IProducerAccessor>().GetProducer(options.Value.Name);
        }

        private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<LineupProducerOptions>(configuration.GetSection(nameof(LineupProducerOptions)))
                .Configure<PassProducerOptions>(configuration.GetSection(nameof(PassProducerOptions)));
            
            return services;
        }

        private static IServiceCollection AddSystemServices(this IServiceCollection services)
        {
            services.AddScoped<ITeamService, TeamService>();

            return services;
        }
    }
}
