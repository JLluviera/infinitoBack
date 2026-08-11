using infinitoBack.Data;
using infinitoBack.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace infinitoBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcursionesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public ExcursionesController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }


        // GET: api/<ExcursionesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ExcursionesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ExcursionesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ExcursionesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ExcursionesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
