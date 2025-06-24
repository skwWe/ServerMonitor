using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApiClient.Models;
using System.ComponentModel.DataAnnotations;

namespace ServerApiClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        private readonly ServerMonitoringContext _context;

        public ErrorsController(ServerMonitoringContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Error>>> GetErrors()
        {
            return await _context.Errors.ToListAsync();
        }

        [HttpGet("server_block")]
        public async Task<ActionResult<IEnumerable<Error>>> GetErrors_ErrorBlock()
        {
            return await _context.Errors.Include(x => x.Server).ThenInclude(x => x.Block).ToListAsync();
        }

        [HttpGet("ByImportance")]
        public async Task<ActionResult<IEnumerable<Error>>> GetErrors_ByImportance([FromQuery] int? importance = null)
        {
            return await _context.Errors.Where(x => x.Importance == importance).ToListAsync();
        }

        [HttpGet("ByServer")]
        public async Task<ActionResult<IEnumerable<Error>>> GetErrors_ByServer([FromQuery] Guid server)
        {
            return await _context.Errors.Include(x => x.Server).Where(x => x.ServerId == server).ToListAsync();
        }

        [HttpGet("byState")]
        public async Task<ActionResult<IEnumerable<Error>>> GetErrors_ByState([FromQuery] bool state)
        {
            return await _context.Errors.Include(x => x.Server).ThenInclude(x => x.Block).Where(x => x.State == state).ToListAsync();
        }

        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<Error>>> GetRecentErrors([FromQuery] int minutesAgo)
        {
            if (minutesAgo != 5 && minutesAgo != 10 && minutesAgo != 15)
            {
                return BadRequest("Допустимые значения: 5, 10 или 15 минут.");
            }

            var targetTime = DateTime.UtcNow.AddMinutes(-minutesAgo);

                var recentErrors = await _context.Errors
            .Where(e => e.CreatedAt >= targetTime) 
            .ToListAsync();

            return recentErrors;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Error>> GetErrorById(Guid id)
        {
            Error? error = await _context.Errors.FindAsync(id);

            return error == null ? (ActionResult<Error>)NotFound() : (ActionResult<Error>)error;
        }
    }
}

