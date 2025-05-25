using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace KOT_Schedulo.Command;

public class RemindCommandHandler
{
    private readonly ILogger<DiscordBot> _logger;

    public RemindCommandHandler(ILogger<DiscordBot> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(SocketSlashCommand command)
    {
        var message = command.Data.Options.First(x => x.Name == "message").Value.ToString();
        var minutes = Convert.ToInt32(command.Data.Options.First(x => x.Name == "minutes").Value);

        if (minutes <= 0 || minutes > 1440)
        {
            await command.RespondAsync("❌ Số phút phải từ 1 đến 1440 (24 giờ)!", ephemeral: true);
            return;
        }

        await command.RespondAsync($"⏰ Đã đặt nhắc nhở trong {minutes} phút!");

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