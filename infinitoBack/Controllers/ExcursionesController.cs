using infinitoBack.Data;
using infinitoBack.DTOs;
using infinitoBack.Interfaces;
using infinitoBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using infinitoBack.ResponseDTOs;

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
        public List<ExcursionResponseDTO> Get()
        {
            List<ExcursionResponseDTO> excursionesResponse = _context.Excursiones.Select(e => new ExcursionResponseDTO
            {
                Nombre = e.Nombre,
                CantLugares = e.CantLugares,
                CantDias = e.CantDias,
                FechaSalida = e.FechaSalida,
                DestinoId = e.DestinoId
            }).ToList();

            return excursionesResponse;
        }

        // GET api/<ExcursionesController>/5
        [HttpGet("{id}")]
        public ExcursionesResponseDTO Get(int id)
        {
            ExcursionesResponseDTO? excursion = _context.Excursiones
                                                .Include(e => e.Destino)
                                                .Include(e => e.Paquetes)
                                                .Select(e => new ExcursionesResponseDTO
                                                {
                                                    Id = e.Id,
                                                    Nombre = e.Nombre,
                                                    CantLugares = e.CantLugares,
                                                    CantDias = e.CantDias,
                                                    FechaSalida = e.FechaSalida,
                                                    Destino = new DestinoCrearDto
                                                    {
                                                        Nombre = e.Destino.Nombre,
                                                        Ciudad = e.Destino.Ciudad,
                                                        Descripcion = e.Destino.Descripcion,
                                                    },
                                                    Paquetes = e.Paquetes.Select(p => new PaqueteResponseDTO
                                                    {
                                                        Nombre = p.Nombre,
                                                        Precio = p.Precio
                                                    }).ToList()
                                                })
                                                .FirstOrDefault(e => e.Id == id);

            if (excursion == null)
            {
                NotFound();
            }

            return excursion!;
        }

        // POST api/<ExcursionesController>
        [HttpPost]
        public IActionResult Post([FromBody] ExcursionResponseDTO excursion)
        {
            if (excursion == null)
            {
                return BadRequest("Debe proporcionar los datos de la excursión");
            }

            if (excursion!.CantLugares <= 0)
            {
                return BadRequest("La cantidad de lugares debe ser mayor a cero");

            }
            else if (excursion.CantDias <= 0)
            {
                return BadRequest("La cantidad de días debe ser mayor a cero");

            }
            else if (excursion.FechaSalida < DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest("La fecha de salida no puede ser anterior a la fecha actual");

            }

            Destino? destino = _context.Destinos.Find(excursion.DestinoId);

            if (destino == null)
            {
                return BadRequest("El destino proporcionado no existe");
            }
            ;

            Excursion nuevaExcursion = new Excursion();

            nuevaExcursion.Nombre = excursion.Nombre;
            nuevaExcursion.CantLugares = excursion.CantLugares;
            nuevaExcursion.CantDias = excursion.CantDias;
            nuevaExcursion.FechaSalida = excursion.FechaSalida;
            nuevaExcursion.DestinoId = excursion.DestinoId;
            
            _context.Excursiones.Add(nuevaExcursion);
            _context.SaveChanges();

            return Ok("Excursión creada correctamente");
        }
            // PUT api/<ExcursionesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Excursion excursionMod)
        {
            if ( id == 0 )
            {
                return BadRequest("Debe proporcionar un ID válido");
            }

            Excursion? excursion = _context.Excursiones.Find(id);

            if (excursion == null)
            {
                return NotFound("NO se encontro la excursion");
            }

            if (excursionMod == null)
            {
                return BadRequest("Debe proporcionar los datos de la excursión");
            }

            if (excursionMod!.CantLugares <= 0)
            {
                return BadRequest("La cantidad de lugares debe ser mayor a cero");

            }
            else if (excursionMod.CantDias <= 0)
            {
                return BadRequest("La cantidad de días debe ser mayor a cero");

            }
            else if (excursionMod.FechaSalida < DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest("La fecha de salida no puede ser anterior a la fecha actual");

            }

            Destino? destino = _context.Destinos.Find(excursionMod.DestinoId);

            if (destino == null)
            {
                return BadRequest("El destino proporcionado no existe");
            }
            ;

            excursion.Nombre = excursionMod.Nombre;
            excursion.CantLugares = excursionMod.CantLugares;
            excursion.CantDias = excursionMod.CantDias;
            excursion.FechaSalida = excursionMod.FechaSalida;
            excursion.DestinoId = excursionMod.DestinoId;

            _context.Excursiones.Update(excursion);
            _context.SaveChanges();

            return Ok("Excursión actualizada correctamente");
        }

        // DELETE api/<ExcursionesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Excursion? excursion = _context.Excursiones.Find(id);
            if (excursion == null)
            {
                return NotFound("No se encontro la excursion");
            }
            _context.Excursiones.Remove(excursion);
            _context.SaveChanges();
            return Ok("Excursión eliminada correctamente");
        }
    }
}
