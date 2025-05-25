using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace KOT_Schedulo.Command;

public class PingCommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<DiscordBot> _logger;

    public PingCommandHandler(DiscordSocketClient client, ILogger<DiscordBot> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task HandleAsync(SocketSlashCommand command)
    {
        var latency = _client.Latency;
        var embed = new EmbedBuilder()
            .WithTitle("🏓 Pong!")
            .WithDescription($"Độ trễ: {latency}ms")
            .WithColor(latency < 100 ? Color.Green : latency < 300 ? Color.Orange : Color.Red)
            .WithCurrentTimestamp()
            .Build();

        await command.RespondAsync(embed: embed);
    }
}