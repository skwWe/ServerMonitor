using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApiClient.Models;

namespace ServerApiClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametersController(ServerMonitoringContext context) : ControllerBase
    {
        private readonly ServerMonitoringContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parameter>>> GetServers()
        {
            return await _context.Parameters.ToListAsync();
        }

        [HttpGet("ByName")]
        public async Task<ActionResult<IEnumerable<Parameter>>> GetParameters_ByName([FromQuery] string? name = null)
        {
            return await _context.Parameters.Where(x => x.Name.Contains(name)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Parameter>> GetParameterById(Guid id)
        {
            Parameter? parameter = await _context.Parameters.FindAsync(id);

            return parameter == null ? (ActionResult<Parameter>)NotFound() : (ActionResult<Parameter>)parameter;
        }

    }
}
