using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace KOT_Schedulo.Command;

public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<DiscordBot> _logger;
    private readonly PingCommandHandler _pingCommandHandler;
    private readonly ScheduleCommandHandler _scheduleCommandHandler;
    private readonly RemindCommandHandler _remindCommandHandler;

    public CommandHandler(
        DiscordSocketClient client, 
        ILogger<DiscordBot> logger,
        PingCommandHandler pingCommandHandler,
        ScheduleCommandHandler scheduleCommandHandler,
        RemindCommandHandler remindCommandHandler)
    {
        _client = client;
        _logger = logger;
        _pingCommandHandler = pingCommandHandler;
        _scheduleCommandHandler = scheduleCommandHandler;
        _remindCommandHandler = remindCommandHandler;
    }

    public async Task RegisterSlashCommandsAsync()
    {
        try
        {
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
                .AddOption("time", ApplicationCommandOptionType.String, "Thời gian (dd/MM/yyyy HH:mm)", isRequired: true)
                .AddOption("description", ApplicationCommandOptionType.String, "Mô tả chi tiết", isRequired: false);

            var remindCommand = new SlashCommandBuilder()
                .WithName("remind")
                .WithDescription("Đặt nhắc nhở")
                .AddOption("message", ApplicationCommandOptionType.String, "Nội dung nhắc nhở", isRequired: true)
                .AddOption("minutes", ApplicationCommandOptionType.Integer, "Số phút để nhắc nhở", isRequired: true);

            var pingCommand = new SlashCommandBuilder()
                .WithName("ping")
                .WithDescription("Kiểm tra độ trễ của bot");

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
            _logger.LogInformation("Retrying slash command registration in 10 seconds...");
            await Task.Delay(10000);
            await RegisterSlashCommandsAsync();
        }
    }

    public async Task HandleSlashCommandAsync(SocketSlashCommand command)
    {
        try
        {
            switch (command.Data.Name)
            {
                case "ping":
                    await _pingCommandHandler.HandleAsync(command);
                    break;
                case "schedule":
                    await _scheduleCommandHandler.HandleAsync(command);
                    break;
                case "remind":
                    await _remindCommandHandler.HandleAsync(command);
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
}