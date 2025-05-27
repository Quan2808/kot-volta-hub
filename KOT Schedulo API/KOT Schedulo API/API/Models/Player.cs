using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

public class Player
{
    [Key]
    public int PlayerId { get; set; }

    [Required]
    public int TeamId { get; set; }

    [Required]
    [StringLength(100)]
    public string PlayerName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Position { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("TeamId")]
    public Team Team { get; set; } = null!;

    // public ICollection<Lineup> Lineups { get; set; } = new List<Lineup>();
}