using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class TeamsController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
    {
        return await context.Teams.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Team>> GetTeam(int id)
    {
        if (!await IsExistingTeamAsync(id))
        {
            return NotFound(new { Message = "Team not found." });
        }

        var team = await context.Teams.FindAsync(id);
        return Ok(team);
    }

    [HttpPost]
    public async Task<ActionResult<Team>> SaveTeam(Team team)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Invalid team data." });
        }

        bool isUpdate = team.TeamId > 0;

        if (isUpdate && !await IsExistingTeamAsync(team.TeamId))
        {
            return NotFound(new { Message = "Team not found." });
        }

        if (await IsTeamNameTakenAsync(team.TeamName, isUpdate ? team.TeamId : null))
        {
            return Conflict(new { Message = "A team with the same name already exists." });
        }

        if (isUpdate)
        {
            var existingTeam = await context.Teams.FindAsync(team.TeamId);
            existingTeam!.TeamName = team.TeamName;
            existingTeam.UpdatedAt = DateTime.UtcNow;
            context.Entry(existingTeam).State = EntityState.Modified;
        }
        else
        {
            team.CreatedAt = DateTime.UtcNow;
            context.Teams.Add(team);
        }

        await context.SaveChangesAsync();

        return isUpdate
            ? Ok(new { Message = "Team updated successfully." })
            : CreatedAtAction(nameof(GetTeam), new { id = team.TeamId }, team);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeam(int id)
    {
        if (!await IsExistingTeamAsync(id))
        {
            return NotFound(new { Message = "Team not found." });
        }

        var team = await context.Teams.FindAsync(id);
        context.Teams.Remove(team!);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> IsTeamNameTakenAsync(string teamName, int? excludeTeamId = null)
    {
        return await context.Teams
            .AnyAsync(t =>
                t.TeamName.ToLower() == teamName.ToLower() &&
                (!excludeTeamId.HasValue || t.TeamId != excludeTeamId.Value));
    }

    private async Task<bool> IsExistingTeamAsync(int teamId)
    {
        return await context.Teams.AnyAsync(t => t.TeamId == teamId);
    }
}