using KafkaStreams.Producer.Services;
using KafkaStreams.Producer.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaStreams.Producer
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = Extensions.GetConfiguration();
            IServiceProvider services = Extensions.GetServices(configuration);

            var teamService = services.GetRequiredService<ITeamService>();

            var homeTeam = teamService.GetHomeTeamLineup();
            var awayTeam = teamService.GetAwayTeamLineup();

            await teamService.RevealLineups(homeTeam);
            await teamService.RevealLineups(awayTeam);

            var game = GameService.StartTheGame(homeTeam, awayTeam, services);

            Console.WriteLine("The game has started. To stop the game press the 'Esc' button\n");

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                await game.MakePass();
            }

            Console.WriteLine("\nThe game is over");
        }
    }
}