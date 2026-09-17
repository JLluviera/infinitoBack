using infinitoBack.Data;
using Microsoft.AspNetCore.Mvc;
using infinitoBack.Models;
using infinitoBack.DTOs;
using infinitoBack.Enum;
using Microsoft.EntityFrameworkCore;

namespace infinitoBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransaccionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> ObtenerTransacciones()
        {
            List<Transaccion> Transacciones = await _context.Transacciones.ToListAsync();
            return Ok(Transacciones);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerTransacciopnesPorId(int id)
        {
            Transaccion? transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound($"No se encontro la transaccion con id {id}");
            }
            return Ok(transaccion);
        }
        [HttpPost]

        public async Task<IActionResult> CrearTransaccion([FromBody] TransaccionCrearDto transaccionDatos)
        {
            bool existe = await _context.Transacciones.FirstAsync(transaccion=>transaccion.Id = transaccionDatos.Id)
        } 

    }
}