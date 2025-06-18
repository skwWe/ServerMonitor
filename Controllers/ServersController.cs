using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ServerApiClient.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class ServersController : ControllerBase
    {
        private readonly PostgresContext _context;

        public ServersController(PostgresContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Server>>> GetServers()
        {
            return await _context.Servers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Server>> GetServer(Guid id)
        {
            var server = await _context.Servers.FindAsync(id);

            if (server == null)
            {
                return NotFound();
            }

            return server;
        }

        [HttpPost]
        public async Task<ActionResult<Server>> PostServer([FromBody] ServerDto serverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var server = new Server
            {
                NameServer = serverDto.NameServer,
                IpAdress = serverDto.IpAdress,
                IdServerGroup = serverDto.IdServerGroup,
                ServerStatus = serverDto.ServerStatus
            };

            _context.Servers.Add(server);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServer), new { id = server.IdServer }, server);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ServerDto serverDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingServer = await _context.Servers.FindAsync(id);
            if (existingServer == null)
            {
                return NotFound();
            }

            existingServer.NameServer = serverDto.NameServer;
            existingServer.IpAdress = serverDto.IpAdress;
            existingServer.IdServerGroup = serverDto.IdServerGroup;
            existingServer.ServerStatus = serverDto.ServerStatus;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.Servers.FindAsync(id);
            if (item == null) return NotFound();

            _context.Servers.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        public class ServerDto
        {
            public string NameServer { get; set; } = null!;

            public string IpAdress { get; set; } = null!;

            public Guid IdServerGroup { get; set; }

            public bool? ServerStatus { get; set; }
        }
    }
}
