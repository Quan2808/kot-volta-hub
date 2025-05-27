using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    // public DbSet<Competition> Competitions { get; set; }
    // public DbSet<Match> Matches { get; set; }
    // public DbSet<Lineup> Lineups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships or additional constraints if needed
        // modelBuilder.Entity<Match>()
        //     .HasOne(m => m.HomeTeam)
        //     .WithMany(t => t.HomeMatches)
        //     .HasForeignKey(m => m.HomeTeamId);
        //
        // modelBuilder.Entity<Match>()
        //     .HasOne(m => m.AwayTeam)
        //     .WithMany(t => t.AwayMatches)
        //     .HasForeignKey(m => m.AwayTeamId);
        modelBuilder.Entity<Team>()
            .HasIndex(e => e.TeamName)
            .IsUnique();
    }
}