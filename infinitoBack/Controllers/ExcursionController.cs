//using infinitoBack.Data;
//using Microsoft.AspNetCore.Mvc;
//using infinitoBack.DTOs;
//using Microsoft.EntityFrameworkCore;
//using infinitoBack.Models;

//namespace infinitoBack.Controllers;

//[ApiController]
//[Route("api/[controller]")]

//public class ExcursionController : ControllerBase
//{
//    private readonly AppDbContext _context;

//    public ExcursionController(AppDbContext context)
//    {
//        _context = context;
//    }

//    [HttpPost]
//    public async Task<IActionResult> CrearExcursion([FromBody] ExcursionCrearDto ExcursionCrearDto)
//    {
//        bool existe = await _context.Excursiones.AnyAsync(Excursion => Excursion.Nombre == ExcursionCrearDto.Nombre);

//        if (existe)
//        {
//            return Conflict($"Ya existe un Excursion con el nombre {ExcursionCrearDto.Nombre}");
//        }
//        bool destinoExiste = await _context.Destinos.AnyAsync(destino => destino.Id == ExcursionCrearDto.DestinoId);

//        if (!destinoExiste)
//        {
//            return BadRequest("El destino seleccionado no existe.");
//        }

        Excursion Excursion = new Excursion()
        {
            Nombre = ExcursionCrearDto.Nombre,
            Precio = ExcursionCrearDto.Precio,
            Seña = ExcursionCrearDto.Seña,
            FechaSalida = ExcursionCrearDto.FechaSalida,
            DuracionDias = ExcursionCrearDto.CantDias,
            Descripcion = ExcursionCrearDto.Descripcion,
            DestinoId = ExcursionCrearDto.DestinoId
        };

//        await _context.Excursiones.AddAsync(Excursion);
//        await _context.SaveChangesAsync();

//        return Ok($"La Excursion se creo correctamente");
//    }
//    [HttpPut("{id}")]
//    public async Task<IActionResult> EditarExcursion(int id, [FromBody] ExcursionCrearDto ExcursionModificado)
//    {
//        Excursion? Excursion = await _context.Excursiones.FindAsync(id);
//        if (Excursion == null)
//        {
//            return NotFound($"No se encontro ningun Excursion con el id {id}");
//        }
//        bool destinoExiste = await _context.Destinos.AnyAsync(destino => destino.Id == ExcursionModificado.DestinoId);

//        if (!destinoExiste)
//        {
//            return BadRequest("El destino seleccionado no existe.");
//        }

        Excursion.DuracionDias = ExcursionModificado.CantDias;
        Excursion.Seña = ExcursionModificado.Seña;
        Excursion.DestinoId = ExcursionModificado.DestinoId;
        Excursion.Nombre = ExcursionModificado.Nombre;
        Excursion.Precio = ExcursionModificado.Precio;
        Excursion.FechaSalida = ExcursionModificado.FechaSalida;
        Excursion.Descripcion = ExcursionModificado.Descripcion;

//        await _context.SaveChangesAsync();
//        return Ok($"El Excursion se modifico correctamente");
//    }

//    [HttpDelete("{id}")]
//    public async Task<ActionResult> Borrar(int id)
//    {
//        Excursion? Excursion = await _context.Excursiones.FindAsync(id);
//        if (Excursion == null)
//        {
//            return NotFound($"No se encontro ningun Excursion con el id {id}");
//        }
//        _context.Excursiones.Remove(Excursion);
//        await _context.SaveChangesAsync();

//        return Ok($"El Excursion con id {id} se borro correctamente");
//    }

//    [HttpGet]
//    public async Task<ActionResult> ListarExcursiones()
//    {
//        List<Excursion> Excursiones = await _context.Excursiones.ToListAsync();
//        return Ok(Excursiones);
//    }
    
//    [HttpGet("{id}")]
//    public async Task<IActionResult> ObtenerExcursionPorId(int id)
//    {
//        Excursion? Excursion = await _context.Excursiones.FindAsync(id);

//        if (Excursion == null)
//        {
//            return NotFound($"No se encontró ningún Excursion con el id {id}");
//        }

//        return Ok(Excursion);
//    }

//}
