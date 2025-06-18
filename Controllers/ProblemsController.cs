using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ServerApiClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        public class ProblemsController : ControllerBase
        {
            private readonly PostgresContext _context;

            public ProblemsController(PostgresContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<ProblemDto>>> GetProblems()
            {
                        return await _context.Problems
                .Select(p => new ProblemDto
                {
                    IdProblem = p.IdProblem,
                    DateTimeProblem = p.DateTimeProblem,
                    DateProblemSolution = p.DateProblemSolution,
                    ErrorImportance = p.ErrorImportance,
                    StatusProblem = p.StatusProblem,
                    IdServer = p.IdServer,
                    MessageProblem = p.MessageProblem
                })
                .ToListAsync();
            }


            [HttpGet("{id}")]
            public async Task<ActionResult<ProblemDto>> GetProblem(Guid id)
            {
                var problem = await _context.Problems
                .Where(p => p.IdProblem == id)
                .Select(p => new ProblemDto
                {
                    IdProblem = p.IdProblem,
                    DateTimeProblem = p.DateTimeProblem,
                    DateProblemSolution = p.DateProblemSolution,
                    ErrorImportance = p.ErrorImportance,
                    StatusProblem = p.StatusProblem,
                    IdServer = p.IdServer,
                    MessageProblem = p.MessageProblem
                })
                .FirstOrDefaultAsync();

                if (problem == null)
                {
                    return NotFound();
                }

                return problem;
            }

            [HttpPost]
            public async Task<ActionResult<Problem>> AddProblem([FromBody] ProblemDto problemDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var problem = new Problem
                {
                    DateTimeProblem = problemDto.DateTimeProblem,
                    DateProblemSolution = problemDto.DateProblemSolution,
                    ErrorImportance = problemDto.ErrorImportance,
                    StatusProblem = problemDto.StatusProblem,
                    IdServer = problemDto.IdServer,
                    MessageProblem = problemDto.MessageProblem,
                };

                _context.Add(problem);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProblem), new { id = problem.IdProblem }, problem);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateProblem(Guid id, [FromBody] ProblemDto problemDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingProblem = await _context.Problems.FindAsync(id);
                if (existingProblem == null)
                {
                    return NotFound();
                }

                existingProblem.DateTimeProblem = problemDto.DateTimeProblem;
                existingProblem.DateProblemSolution = problemDto.DateProblemSolution;
                existingProblem.ErrorImportance = problemDto.ErrorImportance;
                existingProblem.StatusProblem = problemDto.StatusProblem;
                existingProblem.IdServer = problemDto.IdServer;
                existingProblem.MessageProblem = problemDto.MessageProblem;
                
                await _context.SaveChangesAsync();
                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteProblem(Guid id)
            {
                var item = await _context.Problems.FindAsync(id);
                if (item == null) return NotFound();

                _context.Problems.Remove(item);
                await _context.SaveChangesAsync();
                return NoContent();
            }

        public class ProblemDto
        {
            public Guid IdProblem { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime DateTimeProblem { get; set; }
            public DateTime? DateProblemSolution { get; set; }
            public int ErrorImportance { get; set; }
            public string ErrorImportanceText => GetImportanceText(ErrorImportance);
                public bool StatusProblem { get; set; }
                public Guid IdServer { get; set; }
                public string MessageProblem { get; set; }

                public static string GetImportanceText(int level)
                {
                    return level switch
                    {
                        1 => "Низкая",
                        2 => "Нормальная",
                        3 => "Высокая",
                        4 => "Критическая",
                        5 => "Срочная",
                        6 => "Аварийная",
                        _ => "Неизвестный уровень"
                    };
                }
            }
        }
}

