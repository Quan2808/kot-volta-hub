using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Team
{
    [Key]
    public int TeamId { get; set; }

    [Required]
    [StringLength(100)]
    public string TeamName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Player> Players { get; set; } = new List<Player>();
    // public ICollection<Match> HomeMatches { get; set; } = new List<Match>();
    // public ICollection<Match> AwayMatches { get; set; } = new List<Match>();
    // public ICollection<Lineup> Lineups { get; set; } = new List<Lineup>();
}