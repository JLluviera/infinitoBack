using Microsoft.AspNetCore.Mvc;
using infinitoBack.Data;
using Microsoft.EntityFrameworkCore;
using infinitoBack.Models;

namespace infinitoBack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuarioController(AppDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public IActionResult ObtenerUsuarios()
    {
        List<Usuario> listaUsuario = _context.Usuarios.ToList();
        return Ok(listaUsuario);
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerUsuarioPorId(int id)
    {
        Usuario usuario = _context.Usuarios.Find(id);
        if (usuario == null)
        {
            return NotFound($"No se econtro ningun usuario con el id {id}");
        }
        return Ok(usuario);
    }
}