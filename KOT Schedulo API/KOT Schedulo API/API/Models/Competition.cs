using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Competition
{
    [Key]
    public int CompetitionId { get; set; }

    [Required]
    [StringLength(100)]
    public string CompetitionName { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Season { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [StringLength(50)]
    public string? CompetitionType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}
