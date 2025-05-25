using Discord;
using Discord.WebSocket;
using KOT_Schedulo.Command;
using KOT_Schedulo.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EventHandler = KOT_Schedulo.Event.EventHandler;

namespace KOT_Schedulo;

public class DiscordBot
{
    private readonly DiscordSocketClient _client;
    private readonly BotConfig _config;
    private readonly ILogger<DiscordBot> _logger;
    private readonly EventHandler _eventHandler;
    private readonly CommandHandler _commandHandler;

    public DiscordBot(
        DiscordSocketClient client, 
        IOptions<BotConfig> config, 
        ILogger<DiscordBot> logger,
        EventHandler eventHandler,
        CommandHandler commandHandler)
    {
        _client = client;
        _config = config.Value;
        _logger = logger;
        _eventHandler = eventHandler;
        _commandHandler = commandHandler;
    }

    public async Task StartAsync()
    {
        // Subscribe to events
        _client.Log += _eventHandler.LogAsync;
        _client.Ready += _eventHandler.ReadyAsync;
        _client.MessageReceived += _eventHandler.MessageReceivedAsync;
        _client.SlashCommandExecuted += _commandHandler.HandleSlashCommandAsync;

        await _client.LoginAsync(TokenType.Bot, _config.Token);
        await _client.StartAsync();
        
        // Wait for the bot to be ready
        await Task.Delay(3000);
    }
}