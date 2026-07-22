using infinitoBack.Data;
using infinitoBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace infinitoBack.Controllers
{
    [ApiController]
    [Route("api[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServicioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CrearServicio([FromBody] Servicio servicioARegistrar)
        {
            bool existe = await _context.Servicios.AnyAsync(servicio => servicio.Nombre == servicioARegistrar.Nombre);
            if (existe)
            {
                return Conflict($"Ya existe un servicio registrado con el nombre {servicioARegistrar.Nombre}");
            }
            Servicio servicio = new Servicio()
            {
                Nombre = servicioARegistrar.Nombre,
                Descripcion = servicioARegistrar.Descripcion
            };
            return Ok("El servicio se registro correctamente");

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarServicio([FromBody] ServicioEditarDto servicioModificado,int id)
        {
            Servicio? servicio = await _context.Servicios.FindAsync(id);
            if(servicio == null)
            {
                return NotFound($"El  servicio con id {id} no existe");
            }
            servicio.Descripcion = servicioModificado.Descripcion;
            servicio.Nombre = servicioModificado.Nombre;

            await _context.SaveChangesAsync();
            return Ok($"El Servicio con id {id} fue modificado con exito!");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarServicio(int id) {
            Servicio? servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound($"No se econtro ningun servicio con el id {id}");
            }

            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();
            return Ok($"El servicio con el id {id} fue eliminado con exito");
        }
        [HttpGet]
        public async Task<IActionResult> ListarServicios()
        {
            List <Servicio> servicios = await _context.Servicios.ToListAsync();
            return Ok(servicios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerServicioPorId(int id)
        {
            Servicio? servicio = await _context.Servicios.FindAsync(id);

            if (servicio == null)
            {
                return NotFound($"No se encontró ningún paquete con el id {id}");
            }

            return Ok(servicio);
        }
    }
}
