using Discord;
using Discord.WebSocket;
using KOT_Schedulo.Command;
using Microsoft.Extensions.Logging;

namespace KOT_Schedulo.Event;

public class EventHandler
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<DiscordBot> _logger;
    private readonly CommandHandler _commandHandler;

    public EventHandler(DiscordSocketClient client, ILogger<DiscordBot> logger, CommandHandler commandHandler)
    {
        _client = client;
        _logger = logger;
        _commandHandler = commandHandler;
    }

    public Task LogAsync(LogMessage log)
    {
        var severity = log.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Debug,
            LogSeverity.Debug => LogLevel.Trace,
            _ => LogLevel.Information
        };

        _logger.Log(severity, log.Exception, "[{Source}] {Message}", log.Source, log.Message);
        return Task.CompletedTask;
    }

    public async Task ReadyAsync()
    {
        _logger.LogInformation("Bot {BotName} is connected and ready!", _client.CurrentUser.Username);

        await _client.SetActivityAsync(new Game("KOT Schedulo - Quản lý lịch trình"));
        await _client.SetStatusAsync(UserStatus.Online);

        await _commandHandler.RegisterSlashCommandsAsync();
    }

    public async Task MessageReceivedAsync(SocketMessage message)
    {
        if (message is not SocketUserMessage userMessage || userMessage.Author.IsBot)
            return;

        if (userMessage.Content.StartsWith("!hello"))
        {
            await userMessage.Channel.SendMessageAsync($"Xin chào {userMessage.Author.Mention}! Tôi là KOT Schedulo Bot 🤖");
        }
        else if (userMessage.Content.StartsWith("!help"))
        {
            var embed = new EmbedBuilder()
                .WithTitle("KOT Schedulo - Trợ giúp")
                .WithDescription("Danh sách các lệnh có sẵn:")
                .AddField("!hello", "Chào hỏi bot", true)
                .AddField("!help", "Hiển thị trợ giúp", true)
                .AddField("/schedule", "Tạo lịch trình mới", true)
                .AddField("/remind", "Đặt nhắc nhở", true)
                .WithColor(Color.Blue)
                .WithCurrentTimestamp()
                .Build();

            await userMessage.Channel.SendMessageAsync(embed: embed);
        }
    }
}