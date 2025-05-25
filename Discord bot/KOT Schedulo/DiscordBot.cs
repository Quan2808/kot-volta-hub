using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KOT_Schedulo;

public class DiscordBot
{
    private readonly DiscordSocketClient _client;
    private readonly BotConfig _config;
    private readonly ILogger<DiscordBot> _logger;

    public DiscordBot(DiscordSocketClient client, IOptions<BotConfig> config, ILogger<DiscordBot> logger)
    {
        _client = client;
        _config = config.Value;
        _logger = logger;
        
        // Subscribe to events
        _client.Log += LogAsync;
        _client.Ready += ReadyAsync;
        _client.MessageReceived += MessageReceivedAsync;
        _client.SlashCommandExecuted += SlashCommandHandler;
    }

    public async Task StartAsync()
    {
        await _client.LoginAsync(TokenType.Bot, _config.Token);
        await _client.StartAsync();
        
        // Wait for the bot to be ready before continuing
        await Task.Delay(3000);
    }

    private async Task LogAsync(LogMessage log)
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
    }

    private async Task ReadyAsync()
    {
        _logger.LogInformation("Bot {BotName} is connected and ready!", _client.CurrentUser.Username);
        
        // Set bot status
        await _client.SetActivityAsync(new Game("KOT Schedulo - Quản lý lịch trình"));
        await _client.SetStatusAsync(UserStatus.Online);
        
        // Register slash commands after bot is ready
        await RegisterSlashCommandsAsync();
    }

    private async Task MessageReceivedAsync(SocketMessage message)
    {
        // Ignore system messages and bot messages
        if (message is not SocketUserMessage userMessage || userMessage.Author.IsBot)
            return;

        // Simple text commands
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

    private async Task RegisterSlashCommandsAsync()
    {
        try
        {
            // Check if bot is ready
            if (_client.CurrentUser == null)
            {
                _logger.LogWarning("Bot is not ready yet, skipping slash command registration");
                return;
            }

            _logger.LogInformation("Registering slash commands...");

            var scheduleCommand = new SlashCommandBuilder()
                .WithName("schedule")
                .WithDescription("Tạo lịch trình mới")
                .AddOption("title", ApplicationCommandOptionType.String, "Tiêu đề lịch trình", isRequired: true)
                .AddOption("time", ApplicationCommandOptionType.String, "Thời gian (dd/MM/yyyy HH:mm)",
                    isRequired: true)
                .AddOption("description", ApplicationCommandOptionType.String, "Mô tả chi tiết", isRequired: false);

            var remindCommand = new SlashCommandBuilder()
                .WithName("remind")
                .WithDescription("Đặt nhắc nhở")
                .AddOption("message", ApplicationCommandOptionType.String, "Nội dung nhắc nhở", isRequired: true)
                .AddOption("minutes", ApplicationCommandOptionType.Integer, "Số phút để nhắc nhở", isRequired: true);

            var pingCommand = new SlashCommandBuilder()
                .WithName("ping")
                .WithDescription("Kiểm tra độ trễ của bot");

            // Create commands one by one with delay to avoid rate limiting
            await _client.CreateGlobalApplicationCommandAsync(scheduleCommand.Build());
            await Task.Delay(1000);
            
            await _client.CreateGlobalApplicationCommandAsync(remindCommand.Build());
            await Task.Delay(1000);
            
            await _client.CreateGlobalApplicationCommandAsync(pingCommand.Build());

            _logger.LogInformation("Slash commands registered successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering slash commands");
            
            // Retry after delay if failed
            _logger.LogInformation("Retrying slash command registration in 10 seconds...");
            await Task.Delay(10000);
            await RegisterSlashCommandsAsync();
        }
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        try
        {
            switch (command.Data.Name)
            {
                case "ping":
                    await HandlePingCommand(command);
                    break;
                case "schedule":
                    await HandleScheduleCommand(command);
                    break;
                case "remind":
                    await HandleRemindCommand(command);
                    break;
                default:
                    await command.RespondAsync("Lệnh không được hỗ trợ!", ephemeral: true);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling slash command: {CommandName}", command.Data.Name);
            await command.RespondAsync("Đã xảy ra lỗi khi xử lý lệnh!", ephemeral: true);
        }
    }

    private async Task HandlePingCommand(SocketSlashCommand command)
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

    private async Task HandleScheduleCommand(SocketSlashCommand command)
    {
        var title = command.Data.Options.First(x => x.Name == "title").Value.ToString();
        var timeStr = command.Data.Options.First(x => x.Name == "time").Value.ToString();
        var description = command.Data.Options.FirstOrDefault(x => x.Name == "description")?.Value?.ToString() ?? "Không có mô tả";

        if (!DateTime.TryParseExact(timeStr, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out var scheduledTime))
        {
            await command.RespondAsync("❌ Định dạng thời gian không hợp lệ! Vui lòng sử dụng: dd/MM/yyyy HH:mm", ephemeral: true);
            return;
        }

        var embed = new EmbedBuilder()
            .WithTitle("📅 Lịch trình đã được tạo!")
            .WithDescription($"**{title}**")
            .AddField("⏰ Thời gian", scheduledTime.ToString("dd/MM/yyyy HH:mm"), true)
            .AddField("👤 Người tạo", command.User.Mention, true)
            .AddField("📝 Mô tả", description, false)
            .WithColor(Color.Green)
            .WithCurrentTimestamp()
            .Build();

        await command.RespondAsync(embed: embed);
        
        _logger.LogInformation("Schedule created: {Title} at {Time} by {User}", title, scheduledTime, command.User.Username);
    }

    private async Task HandleRemindCommand(SocketSlashCommand command)
    {
        var message = command.Data.Options.First(x => x.Name == "message").Value.ToString();
        var minutes = Convert.ToInt32(command.Data.Options.First(x => x.Name == "minutes").Value);

        if (minutes <= 0 || minutes > 1440) // Max 24 hours
        {
            await command.RespondAsync("❌ Số phút phải từ 1 đến 1440 (24 giờ)!", ephemeral: true);
            return;
        }

        await command.RespondAsync($"⏰ Đã đặt nhắc nhở trong {minutes} phút!");

        // Set reminder (simple implementation with Timer)
        var timer = new Timer(async _ =>
        {
            try
            {
                var embed = new EmbedBuilder()
                    .WithTitle("🔔 Nhắc nhở!")
                    .WithDescription(message)
                    .AddField("👤 Người đặt", command.User.Mention, true)
                    .WithColor(Color.Gold)
                    .WithCurrentTimestamp()
                    .Build();

                await command.Channel.SendMessageAsync(embed: embed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending reminder");
            }
        }, null, TimeSpan.FromMinutes(minutes), Timeout.InfiniteTimeSpan);

        _logger.LogInformation("Reminder set: '{Message}' in {Minutes} minutes by {User}", message, minutes, command.User.Username);
    }
}