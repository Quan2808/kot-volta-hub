using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

public class Lineup
{
    [Key]
    public int LineupId { get; set; }

    [Required]
    public int MatchId { get; set; }

    [Required]
    public int TeamId { get; set; }

    [Required]
    public int PlayerId { get; set; }

    [Required]
    [StringLength(50)]
    public string Position { get; set; } = string.Empty;

    public bool IsStarting { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("MatchId")]
    public Match Match { get; set; } = null!;

    [ForeignKey("TeamId")]
    public Team Team { get; set; } = null!;

    [ForeignKey("PlayerId")]
    public Player Player { get; set; } = null!;
}