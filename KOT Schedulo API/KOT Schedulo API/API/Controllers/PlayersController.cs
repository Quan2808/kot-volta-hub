using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class PlayersController(ApplicationDbContext context) : ControllerBase
{
    // GET: api/Players
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        return await context.Players.Include(p => p.Team).ToListAsync();
    }

    // GET: api/Players/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(int id)
    {
        var player = await context.Players.Include(p => p.Team).FirstOrDefaultAsync(p => p.PlayerId == id);
        if (player == null)
        {
            return NotFound();
        }

        return player;
    }

    // POST: api/Players
    [HttpPost]
    public async Task<ActionResult<Player>> CreatePlayer(Player player)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        context.Players.Add(player);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPlayer), new { id = player.PlayerId }, player);
    }

    // PUT: api/Players/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer(int id, Player player)
    {
        if (id != player.PlayerId || !ModelState.IsValid)
        {
            return BadRequest();
        }

        var existingPlayer = await context.Players.FindAsync(id);
        if (existingPlayer == null)
        {
            return NotFound();
        }

        existingPlayer.PlayerName = player.PlayerName;
        existingPlayer.Position = player.Position;
        existingPlayer.TeamId = player.TeamId;
        existingPlayer.UpdatedAt = DateTime.UtcNow;

        context.Entry(existingPlayer).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Players/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(int id)
    {
        var player = await context.Players.FindAsync(id);
        if (player == null)
        {
            return NotFound();
        }

        context.Players.Remove(player);
        await context.SaveChangesAsync();

        return NoContent();
    }
}