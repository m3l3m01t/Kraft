using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kraft.Shared;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kraft.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaClusterController : ControllerBase
    {
        public KafkaClusterController()
        {
         
        }
        // GET: api/<KafkaClusterController>
        [HttpGet]
        public IEnumerable<KafkaCluster> Get()
        {
            throw new NotImplementedException();
        }

        // GET api/<KafkaClusterController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<KafkaClusterController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<KafkaClusterController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<KafkaClusterController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
