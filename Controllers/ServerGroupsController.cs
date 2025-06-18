using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ServerApiClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersGroupsController : ControllerBase
    {
        private readonly PostgresContext _context;

        private async Task<bool> ServersGroupExists(Guid id)
        {
            return await _context.ServersGroups
                .AnyAsync(e => e.IdServerGroup == id);
        }
        public ServersGroupsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/ServersGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServersGroup>>> GetServersGroups()
        {
            return await _context.ServersGroups
                .Include(sg => sg.Servers)
                .ToListAsync();
        }

        // GET: api/ServersGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServersGroup>> GetServersGroup(Guid id)
        {
            var serversGroup = await _context.ServersGroups
                .Include(sg => sg.Servers)
                .FirstOrDefaultAsync(sg => sg.IdServerGroup == id);

            if (serversGroup == null)
            {
                return NotFound();
            }

            return serversGroup;
        }

        
        [HttpPost]
        public async Task<ActionResult<ServersGroup>> PostServersGroup([FromBody] ServerGroupsDto serversGroupDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serversGroup = new ServersGroup
            {
                NameServerGroup = serversGroupDto.NameServerGroup
            };
            
            _context.ServersGroups.Add(serversGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServersGroup", new { id = serversGroup.IdServerGroup }, serversGroup);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutServersGroup(Guid id, [FromBody] ServerGroupsDto serversGroupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingServerGroup = await _context.ServersGroups.FindAsync(id);
            if (existingServerGroup == null)
            {
                NotFound();
            }

            existingServerGroup.NameServerGroup = serversGroupDto.NameServerGroup;


            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.ServersGroups.FindAsync(id);
            if (item == null) return NotFound();

            _context.ServersGroups.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        public class ServerGroupsDto
        {
            public string NameServerGroup { get; set; } = null!;
        }
    }
}
