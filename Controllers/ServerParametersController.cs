using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApiClient.Models;
using System.Collections;

namespace ServerApiClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerParametersController(ServerMonitoringContext context) : ControllerBase
    {
        private readonly ServerMonitoringContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServerParameter>>> GetServerParameters()
        {
            return await _context.ServerParameters.ToListAsync();
        }

        [HttpGet("ByServer")]
        public async Task<ActionResult<IEnumerable<ServerParameter>>> GetServerParametersByServer([FromQuery] Guid server)
        {
            return await _context.ServerParameters.Include(x => x.Server).Where(x => x.ServerId == server).ToListAsync();
        }

        [HttpGet("ByParameter")]
        public async Task<ActionResult<IEnumerable<ServerParameter>>> GetServerParametersByParameter(Guid parameter)
        {
            return await _context.ServerParameters.Include(x => x.Parameter).Where(x => x.ParameterId == parameter).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerParameter>> GetServerParameter(Guid id)
        {
            ServerParameter? serverParameter = await _context.ServerParameters.FindAsync(id);

            return serverParameter == null ? (ActionResult<ServerParameter>)NotFound() : (ActionResult<ServerParameter>)serverParameter;
        }

        [HttpPost]
        public async Task<ActionResult<ServerParameter>> PostServerParameter([FromBody] ServerParameter serverParameter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _ = _context.ServerParameters.Add(serverParameter);
            _ = await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServerParameters), new { id = serverParameter.Id }, serverParameter);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutServerParameter(Guid id, [FromBody] ServerParameter serverParameter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ServerParameter? existingServerParameter = await _context.ServerParameters.FindAsync(id);
            if (existingServerParameter == null)
            {
                return NotFound();
            }

            existingServerParameter.ServerId = serverParameter.ServerId;
            existingServerParameter.ParameterId = serverParameter.ParameterId;


            _ = await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServerParameter(Guid id)
        {
            ServerParameter? serverParameter = await _context.ServerParameters.FindAsync(id);
            if (serverParameter == null)
            {
                return NotFound();
            }
            
            _ = _context.ServerParameters.Remove(serverParameter);
            _ = await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
