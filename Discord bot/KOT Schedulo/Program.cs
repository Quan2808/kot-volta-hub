using Discord.WebSocket;
using KOT_Schedulo.Command;
using KOT_Schedulo.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EventHandler = KOT_Schedulo.Event.EventHandler;

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
                services.AddSingleton<EventHandler>();
                services.AddSingleton<CommandHandler>();
                services.AddSingleton<PingCommandHandler>();
                services.AddSingleton<ScheduleCommandHandler>();
                services.AddSingleton<RemindCommandHandler>();
                services.Configure<BotConfig>(context.Configuration.GetSection("Discord"));
            });
}