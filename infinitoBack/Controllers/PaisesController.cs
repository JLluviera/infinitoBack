using infinitoBack.Data;
using infinitoBack.DTOs;
using infinitoBack.ResponseDTOs;
using infinitoBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace infinitoBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaisesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<PaisesController>
        [HttpGet]
        public List<ResponsePaises> Get()
        {
            List<ResponsePaises> paisesResponse = _context.Paises
                .Select(p => new ResponsePaises
                {
                    Id = p.Id,
                    NombrePais = p.NombrePais,
                    CodigoPais = p.CodigoPais
                })
                .ToList();

            return paisesResponse;
        }

        // GET api/<PaisesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            ResponsePaises? pais = _context.Paises
                                    .Include(p => p.Destinos)
                                    .Select(p => new ResponsePaises
                                    {
                                        Id = p.Id,
                                        NombrePais = p.NombrePais,
                                        CodigoPais = p.CodigoPais,
                                        Destinos = p.Destinos.Select(d => new DestinoCrearDto
                                                                        { Nombre = d.Nombre,
                                                                          Ciudad = d.Ciudad

                                        }).ToList()
                                    })
                                    .FirstOrDefault(p => p.Id == id);
        
            if (pais == null) {
                return NotFound("No se encontro el país");
            }

            return Ok(pais);
        }

        // POST api/<PaisesController>
        [HttpPost]
        public IActionResult Post([FromBody] PaisCrearDTO pais)
        {
            if (pais == null)
            {
                return BadRequest("Debe proporcionar los datos del país");
            } else if (pais.Nombre == null)
            {
                return BadRequest("Debe proporcionar el nombre del país");
            } else if (pais.CodigoPais == null)
            {
                return BadRequest("Debe proporcionar el código del país");
            }

            Pais nuevoPais = new Pais();

            nuevoPais.NombrePais = pais.Nombre;
            nuevoPais.CodigoPais = pais.CodigoPais;
                
            _context.Paises.Add(nuevoPais);
            _context.SaveChanges();
            return Ok(nuevoPais);

        }

        // PUT api/<PaisesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PaisCrearDTO paisMod)
        {
            Pais? pais = _context.Paises.Find(id);

            if (pais == null) {
                return NotFound("No se encontro el pais"); }

            if (paisMod == null)
            {
                return BadRequest("Debe proporcionar los datos del país");
            }
            else if (paisMod.Nombre == null)
            {
                return BadRequest("Debe proporcionar el nombre del país");
            }
            else if (paisMod.CodigoPais == null)
            {
                return BadRequest("Debe proporcionar el código del país");
            }

            pais.NombrePais = paisMod.Nombre;
            pais.CodigoPais = paisMod.CodigoPais;

            _context.Paises.Update(pais);
            _context.SaveChanges();
            return Ok(pais);
        }

        // DELETE api/<PaisesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Pais? pais = _context.Paises.Find(id);

            if (pais == null)
            {
                return NotFound("No se encontro el pais");
            }

            _context.Paises.Remove(pais);
            _context.SaveChanges();
            return Ok(pais);
        }
    }
}
