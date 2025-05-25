using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KOT_Schedulo;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        
        var bot = host.Services.GetRequiredService<DiscordBot>();
        await bot.StartAsync();
        
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<DiscordSocketClient>();
                services.AddSingleton<DiscordBot>();
                services.Configure<BotConfig>(context.Configuration.GetSection("Discord"));
            });
}