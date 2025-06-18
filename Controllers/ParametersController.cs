using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace ServerApiClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametersController : ControllerBase
    {
        private readonly PostgresContext _context;

        public ParametersController(PostgresContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parameter>>> GetParameters()
        {
            return await _context.Parameters
                .Include(p => p.IdServerNavigation)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Parameter>> GetParameterById(Guid id)
        {
            var parameters =  await _context.Parameters
                .Include (p => p.IdServerNavigation)
                .FirstOrDefaultAsync(p => p.RequestId == id);

            if (parameters == null)
            {
                return NotFound();
            }

            return parameters;
        }


        [HttpPost]
        public async Task<ActionResult<Parameter>> AddParameter([FromBody] ParametersDto parameterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parameter = new Parameter
            {
                CreatedAt = parameterDto.CreatedAt,
                IdServer = parameterDto.IdServer,
                RamMb = parameterDto.RamMb,
                CpuPercent = parameterDto.CpuPercent,
                RomMb = parameterDto.RomMb
            };

            _context.Parameters.Add(parameter);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParameters), new { id = parameter.RequestId }, parameter);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParameter(Guid id , [FromBody] ParametersDto parameterDto)
        {
            if(!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            var existingParameter = await _context.Parameters.FindAsync(id);
            if (existingParameter == null)
            {
                return NotFound();
            }

            existingParameter.CreatedAt = parameterDto.CreatedAt;
            existingParameter.IdServer = parameterDto.IdServer;
            existingParameter.RamMb = parameterDto.RamMb;
            existingParameter.CpuPercent = parameterDto.CpuPercent;
            existingParameter.RomMb = parameterDto.RomMb;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParameter(Guid id)
        {
            var item = await _context.Parameters.FindAsync(id);
            if (item == null) return NotFound();

            _context.Parameters.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        public class ParametersDto
        {
            public DateTime CreatedAt { get; set; }

            public Guid IdServer { get; set; }

            public int RamMb { get; set; }

            public int CpuPercent { get; set; }

            public int RomMb { get; set; }
        }
    }
}
