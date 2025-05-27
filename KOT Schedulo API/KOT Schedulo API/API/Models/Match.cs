using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

public class Match
{
    [Key]
    public int MatchId { get; set; }

    [Required]
    public int HomeTeamId { get; set; }

    [Required]
    public int AwayTeamId { get; set; }

    [Required]
    public DateTime MatchDate { get; set; }

    [Required]
    public int CompetitionId { get; set; }

    [StringLength(20)]
    public string MatchStatus { get; set; } = "Scheduled";

    public int HomeTeamScore { get; set; } = 0;

    public int AwayTeamScore { get; set; } = 0;

    [StringLength(50)]
    public string? MatchPhase { get; set; }

    [StringLength(50)]
    public string? DecidedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("HomeTeamId")]
    public Team HomeTeam { get; set; } = null!;

    [ForeignKey("AwayTeamId")]
    public Team AwayTeam { get; set; } = null!;

    [ForeignKey("CompetitionId")]
    public Competition Competition { get; set; } = null!;

    public ICollection<Lineup> Lineups { get; set; } = new List<Lineup>();
}