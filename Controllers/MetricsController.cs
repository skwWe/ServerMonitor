using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApiClient.Models;
using System;

namespace ServerApiClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricsController : ControllerBase
    {
        private readonly ServerMonitoringContext _context;

        public MetricsController(ServerMonitoringContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Metric>>> GetMetric()
        {
            return await _context.Metrics.ToListAsync();
        }

        [HttpGet("ByServer")]
        public async Task<ActionResult<IEnumerable<Metric>>> GetMetrics_ByServer([FromQuery] Guid server)
        {
            return await _context.Metrics.Include(x => x.Server).ThenInclude(x => x.ServerParameters).ThenInclude(x => x.Parameter).Where(x => x.ServerId == server).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Metric>> GetMetricById(Guid id)
        {
            Metric? metric = await _context.Metrics.FindAsync(id);

            return metric == null ? (ActionResult<Metric>)NotFound() : (ActionResult<Metric>)metric;
        }
    }
}
