using infinitoBack.Data;
using Microsoft.AspNetCore.Mvc;
using infinitoBack.DTOs;
using Microsoft.EntityFrameworkCore;
using infinitoBack.Models;

namespace infinitoBack.Controllers;

[ApiController]
[Route("api/[controller]")]

public class PaqueteController : ControllerBase
{
    private readonly AppDbContext _context;

    public PaqueteController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CrearPaquete([FromBody] PaqueteCrearDto paqueteCrearDto)
    {
        bool existe = await _context.Paquetes.AnyAsync(paquete => paquete.Nombre == paqueteCrearDto.Nombre);

        if (existe)
        {
            return Conflict($"Ya existe un paquete con el nombre {paqueteCrearDto.Nombre}");
        }
        bool excursionExiste = await _context.Excursiones.AnyAsync(e=> e.Id == paqueteCrearDto.IdExcursion);

        if (!excursionExiste)
        {
            return BadRequest("La excursión seleccionada no existe.");
        }

        Paquete paquete = new Paquete()
        {
            Nombre = paqueteCrearDto.Nombre,
            Precio = paqueteCrearDto.Precio,
            Seña = paqueteCrearDto.Seña,
            Descripcion = paqueteCrearDto.Descripcion,
            IdExcursion = paqueteCrearDto.IdExcursion
        };

        await _context.Paquetes.AddAsync(paquete);
        await _context.SaveChangesAsync();

        return Ok($"El paquete se creo correctamente");
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> EditarPaquete(int id, [FromBody] PaqueteCrearDto paqueteModificado)
    {
        Paquete? paquete = await _context.Paquetes.FindAsync(id);
        if (paquete == null)
        {
            return NotFound($"No se encontro ningun paquete con el id {id}");
        }
        bool excursionExiste = await _context.Excursiones.AnyAsync(e => e.Id == paqueteModificado.IdExcursion);

        if (!excursionExiste)
        {
            return BadRequest("La excursión seleccionada no existe.");
        }

        paquete.Seña = paqueteModificado.Seña;
        paquete.Nombre = paqueteModificado.Nombre;
        paquete.Precio = paqueteModificado.Precio;
        paquete.Descripcion = paqueteModificado.Descripcion;

        await _context.SaveChangesAsync();
        return Ok($"El paquete se modifico correctamente");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Borrar(int id)
    {
        Paquete? paquete = await _context.Paquetes.FindAsync(id);
        if (paquete == null)
        {
            return NotFound($"No se encontro ningun paquete con el id {id}");
        }
        _context.Paquetes.Remove(paquete);
        await _context.SaveChangesAsync();

        return Ok($"El paquete con id {id} se borro correctamente");
    }

    [HttpGet]
    public async Task<ActionResult> ListarPaquetes()
    {
        List<Paquete> paquetes = await _context.Paquetes.ToListAsync();
        return Ok(paquetes);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPaquetePorId(int id)
    {
        Paquete? paquete = await _context.Paquetes.FindAsync(id);

        if (paquete == null)
        {
            return NotFound($"No se encontró ningún paquete con el id {id}");
        }

        return Ok(paquete);
    }

}
