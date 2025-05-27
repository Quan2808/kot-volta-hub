// using API.Data;
// using API.Models;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
//
// namespace API.Controllers;
//
// [Route("api/[controller]")]
// [ApiController]
// public class LineupsController : ControllerBase
// {
//     private readonly ApplicationDbContext _context;
//
//     public LineupsController(ApplicationDbContext context)
//     {
//         _context = context;
//     }
//
//     // GET: api/Lineups
//     [HttpGet]
//     public async Task<ActionResult<IEnumerable<Lineup>>> GetLineups()
//     {
//         return await _context.Lineups
//             .Include(l => l.Match)
//             .Include(l => l.Team)
//             .Include(l => l.Player)
//             .ToListAsync();
//     }
//
//     // GET: api/Lineups/5
//     [HttpGet("{id}")]
//     public async Task<ActionResult<Lineup>> GetLineup(int id)
//     {
//         var lineup = await _context.Lineups
//             .Include(l => l.Match)
//             .Include(l => l.Team)
//             .Include(l => l.Player)
//             .FirstOrDefaultAsync(l => l.LineupId == id);
//
//         if (lineup == null)
//         {
//             return NotFound();
//         }
//
//         return lineup;
//     }
//
//     // POST: api/Lineups
//     [HttpPost]
//     public async Task<ActionResult<Lineup>> CreateLineup(Lineup lineup)
//     {
//         if (!ModelState.IsValid)
//         {
//             return BadRequest(ModelState);
//         }
//
//         _context.Lineups.Add(lineup);
//         await _context.SaveChangesAsync();
//
//         return CreatedAtAction(nameof(GetLineup), new { id = lineup.LineupId }, lineup);
//     }
//
//     // PUT: api/Lineups/5
//     [HttpPut("{id}")]
//     public async Task<IActionResult> UpdateLineup(int id, Lineup lineup)
//     {
//         if (id != lineup.LineupId || !ModelState.IsValid)
//         {
//             return BadRequest();
//         }
//
//         var existingLineup = await _context.Lineups.FindAsync(id);
//         if (existingLineup == null)
//         {
//             return NotFound();
//         }
//
//         existingLineup.MatchId = lineup.MatchId;
//         existingLineup.TeamId = lineup.TeamId;
//         existingLineup.PlayerId = lineup.PlayerId;
//         existingLineup.Position = lineup.Position;
//         existingLineup.IsStarting = lineup.IsStarting;
//         existingLineup.CreatedAt = lineup.CreatedAt;
//
//         _context.Entry(existingLineup).State = EntityState.Modified;
//         await _context.SaveChangesAsync();
//
//         return NoContent();
//     }
//
//     // DELETE: api/Lineups/5
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteLineup(int id)
//     {
//         var lineup = await _context.Lineups.FindAsync(id);
//         if (lineup == null)
//         {
//             return NotFound();
//         }
//
//         _context.Lineups.Remove(lineup);
//         await _context.SaveChangesAsync();
//
//         return NoContent();
//     }
// }