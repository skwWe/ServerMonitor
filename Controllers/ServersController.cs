using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApiClient.Models;

namespace ServerMonitoringApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServersController(ServerMonitoringContext context) : ControllerBase
{
    private readonly ServerMonitoringContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Server>>> GetServers()
    {
        return await _context.Servers.ToListAsync();
    }

    [HttpGet("error_block")]
    public async Task<ActionResult<IEnumerable<Server>>> GetServers_ErrorBlock()
    {
        return await _context.Servers.Include(x => x.Errors).Include(x => x.Block).ToListAsync();
    }

    [HttpGet("byName")]
    public async Task<ActionResult<IEnumerable<Server>>> GetServers_ByName([FromQuery] string? name = null, [FromQuery] string? ip = null)
    {
        return await _context.Servers.Where(x => x.HostName.Contains(name) || x.IpAddres.Contains(ip)).Include(x => x.Block).Include(x => x.Errors).ToListAsync();
    }


    [HttpGet("byState")]
    public async Task<ActionResult<IEnumerable<Server>>> GetServers_ByState([FromQuery] bool state)
    {
        return await _context.Servers.Include(x => x.Errors).Include(x => x.Block).Where(x => x.State == state).ToListAsync();
    }

    [HttpGet("byBlock")]
    public async Task<ActionResult<IEnumerable<Server>>> GetServers_ByBlock([FromQuery] Guid block)
    {
        return await _context.Servers.Include(x => x.Errors).Include(x => x.Block).Where(x => x.BlockId == block).ToListAsync();
    }

    [HttpGet("byImportance")]
    public async Task<ActionResult<IEnumerable<Server>>> GetServers_ByImportance([FromQuery] int importance)
    {
        return await _context.Servers.Include(x => x.Errors).Include(x => x.Block).Where(x => x.Errors.Any(x => x.Importance == importance)).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Server>> GetServer(Guid id)
    {
        Server? server = await _context.Servers.FindAsync(id);

        return server == null ? (ActionResult<Server>)NotFound() : (ActionResult<Server>)server;
    }

    [HttpPost]
    public async Task<ActionResult<Server>> PostServer([FromBody] Server server)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _ = _context.Servers.Add(server);
        _ = await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetServer), new { id = server.Id }, server);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutServer(Guid id, [FromBody] Server server)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Server? existingServer = await _context.Servers.FindAsync(id);
        if (existingServer == null)
        {
            return NotFound();
        }

        existingServer.HostName = server.HostName;
        existingServer.IpAddres = server.IpAddres;
        existingServer.BlockId = server.BlockId;
        existingServer.State = server.State;

        _ = await _context.SaveChangesAsync();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServer(Guid id)
    {
        Server? server = await _context.Servers.FindAsync(id);
        if (server == null)
        {
            return NotFound();
        }

        _ = _context.Servers.Remove(server);
        _ = await _context.SaveChangesAsync();
        return NoContent();
    }
}