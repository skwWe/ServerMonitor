using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApiClient.Models;

namespace ServerMonitoringApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlocksController(ServerMonitoringContext context) : ControllerBase
{
    private readonly ServerMonitoringContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Block>>> GetBlocks()
    {
        return await _context.Blocks.ToListAsync();
    }

    [HttpGet("server_parameter")]
    public async Task<ActionResult<IEnumerable<Block>>> GetBlocks_ServerParameter()
    {
        return await _context.Blocks.Include(x => x.Servers).ThenInclude(x => x.ServerParameters).ThenInclude(x => x.Parameter).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Block>> GetBlock(Guid id)
    {
        Block? block = await _context.Blocks.FindAsync(id);

        return block == null ? (ActionResult<Block>)NotFound() : (ActionResult<Block>)block;
    }

    [HttpPost]
    public async Task<ActionResult<Block>> PostBlock([FromBody] Block block)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _ = _context.Blocks.Add(block);
        _ = await _context.SaveChangesAsync();

        return CreatedAtAction("GetBlocks", new { id = block.Id }, block);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBlock(Guid id, [FromBody] Block block)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Block? existingBlock = await _context.Blocks.FindAsync(id);
        if (existingBlock == null)
        {
            return NotFound();
        }

        existingBlock.Name = block.Name;

        _ = await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlock(Guid id)
    {
        Block? block = await _context.Blocks.FindAsync(id);
        if (block == null)
        {
            return NotFound();
        }

        _ = _context.Blocks.Remove(block);
        _ = await _context.SaveChangesAsync();
        return NoContent();
    }
}
