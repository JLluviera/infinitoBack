using Microsoft.AspNetCore.Mvc;
using infinitoBack.Data;
using Microsoft.EntityFrameworkCore;
using infinitoBack.Models;
using infinitoBack.DTOs;

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
    public async Task<IActionResult> ObtenerUsuarios()
    {
        List<Usuario> listaUsuario = await _context.Usuarios.ToListAsync();
        return Ok(listaUsuario);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerUsuarioPorId(int id)
    {
        Usuario ?usuario =await  _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound($"No se econtro ningun usuario con el id {id}");
        }
        return Ok(usuario);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> BorrarUsuarioPorId(int id)
    {
        Usuario ?usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound($"No se econtro ningun usuario con el id {id}");
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return Ok($"El usuario con el id {id} fue eliminado con exito");
    }

    [HttpPut("{id}")]

    public async Task<IActionResult> EditarUsuario(int id, [FromBody] UsuarioEditarDto usuarioModificado )
    {
        Usuario ?usuario = await _context.Usuarios.FindAsync(id);
        if(usuario == null)
        {
            return NotFound($"El usuario con id {id} no existe");
        }
        usuario.NombreUsuario = usuarioModificado.Nombre;
        usuario.ApellidoUsuario = usuarioModificado.Apellido;
        usuario.MailUsuario = usuarioModificado.Mail;

        await _context.SaveChangesAsync();
        return Ok($"El usuario con el id {id} fue modificado con exito ");
    }

}