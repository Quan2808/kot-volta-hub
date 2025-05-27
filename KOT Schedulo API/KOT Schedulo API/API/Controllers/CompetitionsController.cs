// using API.Data;
// using API.Models;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
//
// namespace API.Controllers;
//
//     [Route("api/[controller]")]
//     [ApiController]
//     public class CompetitionsController : ControllerBase
//     {
//         private readonly ApplicationDbContext _context;
//
//         public CompetitionsController(ApplicationDbContext context)
//         {
//             _context = context;
//         }
//
//         // GET: api/Competitions
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<Competition>>> GetCompetitions()
//         {
//             return await _context.Competitions.ToListAsync();
//         }
//
//         // GET: api/Competitions/5
//         [HttpGet("{id}")]
//         public async Task<ActionResult<Competition>> GetCompetition(int id)
//         {
//             var competition = await _context.Competitions.FindAsync(id);
//             if (competition == null)
//             {
//                 return NotFound();
//             }
//             return competition;
//         }
//
//         // POST: api/Competitions
//         [HttpPost]
//         public async Task<ActionResult<Competition>> CreateCompetition(Competition competition)
//         {
//             if (!ModelState.IsValid)
//             {
//                 return BadRequest(ModelState);
//             }
//
//             _context.Competitions.Add(competition);
//             await _context.SaveChangesAsync();
//
//             return CreatedAtAction(nameof(GetCompetition), new { id = competition.CompetitionId }, competition);
//         }
//
//         // PUT: api/Competitions/5
//         [HttpPut("{id}")]
//         public async Task<IActionResult> UpdateCompetition(int id, Competition competition)
//         {
//             if (id != competition.CompetitionId || !ModelState.IsValid)
//             {
//                 return BadRequest();
//             }
//
//             var existingCompetition = await _context.Competitions.FindAsync(id);
//             if (existingCompetition == null)
//             {
//                 return NotFound();
//             }
//
//             existingCompetition.CompetitionName = competition.CompetitionName;
//             existingCompetition.Season = competition.Season;
//             existingCompetition.StartDate = competition.StartDate;
//             existingCompetition.EndDate = competition.EndDate;
//             existingCompetition.CompetitionType = competition.CompetitionType;
//             existingCompetition.CreatedAt = competition.CreatedAt;
//
//             _context.Entry(existingCompetition).State = EntityState.Modified;
//             await _context.SaveChangesAsync();
//
//             return NoContent();
//         }
//
//         // DELETE: api/Competitions/5
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteCompetition(int id)
//         {
//             var competition = await _context.Competitions.FindAsync(id);
//             if (competition == null)
//             {
//                 return NotFound();
//             }
//
//             _context.Competitions.Remove(competition);
//             await _context.SaveChangesAsync();
//
//             return NoContent();
//         }
//     }