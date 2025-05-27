// using API.Data;
// using API.Models;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
//
// namespace API.Controllers;
//
// [Route("api/[controller]")]
// [ApiController]
// public class MatchesController : ControllerBase
// {
//     private readonly ApplicationDbContext _context;
//
//     public MatchesController(ApplicationDbContext context)
//     {
//         _context = context;
//     }
//
//     // GET: api/Matches
//     [HttpGet]
//     public async Task<ActionResult<IEnumerable<Match>>> GetMatches()
//     {
//         return await _context.Matches
//             .Include(m => m.HomeTeam)
//             .Include(m => m.AwayTeam)
//             .Include(m => m.Competition)
//             .ToListAsync();
//     }
//
//     // GET: api/Matches/5
//     [HttpGet("{id}")]
//     public async Task<ActionResult<Match>> GetMatch(int id)
//     {
//         var match = await _context.Matches
//             .Include(m => m.HomeTeam)
//             .Include(m => m.AwayTeam)
//             .Include(m => m.Competition)
//             .FirstOrDefaultAsync(m => m.MatchId == id);
//
//         if (match == null)
//         {
//             return NotFound();
//         }
//
//         return match;
//     }
//
//     // POST: api/Matches
//     [HttpPost]
//     public async Task<ActionResult<Match>> CreateMatch(Match match)
//     {
//         if (!ModelState.IsValid)
//         {
//             return BadRequest(ModelState);
//         }
//
//         _context.Matches.Add(match);
//         await _context.SaveChangesAsync();
//
//         return CreatedAtAction(nameof(GetMatch), new { id = match.MatchId }, match);
//     }
//
//     // PUT: api/Matches/5
//     [HttpPut("{id}")]
//     public async Task<IActionResult> UpdateMatch(int id, Match match)
//     {
//         if (id != match.MatchId || !ModelState.IsValid)
//         {
//             return BadRequest();
//         }
//
//         var existingMatch = await _context.Matches.FindAsync(id);
//         if (existingMatch == null)
//         {
//             return NotFound();
//         }
//
//         existingMatch.HomeTeamId = match.HomeTeamId;
//         existingMatch.AwayTeamId = match.AwayTeamId;
//         existingMatch.MatchDate = match.MatchDate;
//         existingMatch.CompetitionId = match.CompetitionId;
//         existingMatch.MatchStatus = match.MatchStatus;
//         existingMatch.HomeTeamScore = match.HomeTeamScore;
//         existingMatch.AwayTeamScore = match.AwayTeamScore;
//         existingMatch.MatchPhase = match.MatchPhase;
//         existingMatch.DecidedBy = match.DecidedBy;
//         existingMatch.CreatedAt = match.CreatedAt;
//
//         _context.Entry(existingMatch).State = EntityState.Modified;
//         await _context.SaveChangesAsync();
//
//         return NoContent();
//     }
//
//     // DELETE: api/Matches/5
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteMatch(int id)
//     {
//         var match = await _context.Matches.FindAsync(id);
//         if (match == null)
//         {
//             return NotFound();
//         }
//
//         _context.Matches.Remove(match);
//         await _context.SaveChangesAsync();
//
//         return NoContent();
//     }
// }