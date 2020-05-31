using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kraft.Server.Data;
using Kraft.Shared;

namespace Kraft.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClustersController : ControllerBase
    {
        private readonly KraftContext _context;

        public ClustersController(KraftContext context)
        {
            _context = context;
        }

        // GET: api/Clusters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cluster>>> GetCluster()
        {
            return await _context.Cluster.ToListAsync();
        }

        // GET: api/Clusters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cluster>> GetCluster(string id)
        {
            var cluster = await _context.Cluster.FindAsync(id);

            if (cluster == null)
            {
                return NotFound();
            }

            return cluster;
        }

        // PUT: api/Clusters/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCluster(string id, Cluster cluster)
        {
            if (id != cluster.Id)
            {
                return BadRequest();
            }

            _context.Entry(cluster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClusterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Clusters
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Cluster>> PostCluster(Cluster cluster)
        {
            _context.Cluster.Add(cluster);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClusterExists(cluster.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCluster", new { id = cluster.Id }, cluster);
        }

        // DELETE: api/Clusters/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cluster>> DeleteCluster(string id)
        {
            var cluster = await _context.Cluster.FindAsync(id);
            if (cluster == null)
            {
                return NotFound();
            }

            _context.Cluster.Remove(cluster);
            await _context.SaveChangesAsync();

            return cluster;
        }

        private bool ClusterExists(string id)
        {
            return _context.Cluster.Any(e => e.Id == id);
        }
    }
}
