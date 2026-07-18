using infinitoBack.Data;
using infinitoBack.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using infinitoBack.Models;
using System.Data;

namespace infinitoBack.Controllers;

[ApiController]
[Route("api/[controller]")]

public class DestinoController : ControllerBase
{
    private readonly AppDbContext _context;

    public DestinoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CrearDestino([FromBody] DestinoCrearDto destinoCrearDto)
    {
        bool existe = await _context.Destinos.AnyAsync(destino =>
            destino.Nombre == destinoCrearDto.Nombre &&
            destino.Ciudad == destinoCrearDto.Ciudad &&
            destino.Pais == destinoCrearDto.Pais);
        if (existe)
        {
            return Conflict("Ya existe un destino con ese nombre en esa ciudad y país.");
        }

        Destino destino = new Destino
        {
            Nombre = destinoCrearDto.Nombre,
            Ciudad = destinoCrearDto.Ciudad,
            Pais = destinoCrearDto.Pais,
            Descripcion = destinoCrearDto.Descripcion
        };

        await _context.Destinos.AddAsync(destino);
        await _context.SaveChangesAsync();


        await _context.Destinos.AddAsync(destino);
        await _context.SaveChangesAsync();
        return Ok("El destino se creo correctamente");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditarDestino(int id, [FromBody] DestinoCrearDto destinoCrearDto)
    {
        Destino? destino = await _context.Destinos.FindAsync(id);
        if (destino == null)
        {
            return NotFound($"El destino con id {id} no existe");
        }
        bool existe = await _context.Destinos.AnyAsync(destino =>
            destino.Id != id &&
            destino.Nombre == destinoCrearDto.Nombre &&
            destino.Ciudad == destinoCrearDto.Ciudad &&
            destino.Pais == destinoCrearDto.Pais);
        if (existe)
        {
            return Conflict("Ya existe un destino con ese nombre en esa ciudad y país.");
        }

        destino.Nombre = destinoCrearDto.Nombre;
        destino.Descripcion = destinoCrearDto.Descripcion;
        destino.Ciudad = destinoCrearDto.Ciudad;
        destino.Pais = destinoCrearDto.Pais;

        await _context.SaveChangesAsync();
        return Ok($"El destino con el id {id} fue modificado con exito ");
    }

    [HttpDelete]
    public async Task<ActionResult> Borrar(int id)
    {
        Destino? destino = await _context.Destinos.FindAsync(id);
        if (destino == null)
        {
            return NotFound($"No se encontro ningun destino con el id{id}");
        }
        _context.Destinos.Remove(destino);
        await _context.SaveChangesAsync();
        return Ok($"El destino con id {id} se borro correctamente");
    }

    [HttpGet]
    public async Task<IActionResult> ListarDestinos()
    {
        List<Destino> destinos = await _context.Destinos.ToListAsync();
        return Ok(destinos);
    }

}
