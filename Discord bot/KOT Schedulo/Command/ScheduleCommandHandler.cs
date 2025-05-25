using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace KOT_Schedulo.Command;

public class ScheduleCommandHandler
{
    private readonly ILogger<DiscordBot> _logger;

    public ScheduleCommandHandler(ILogger<DiscordBot> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(SocketSlashCommand command)
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
}