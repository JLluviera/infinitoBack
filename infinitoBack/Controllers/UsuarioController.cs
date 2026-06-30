using Microsoft.AspNetCore.Mvc;
using infinitoBack.Data;
using Microsoft.EntityFrameworkCore;
using infinitoBack.Models;
using infinitoBack.DTOs;
using infinitoBack.Services;
using Microsoft.IdentityModel.Tokens;
using infinitoBack.Interfaces;

namespace infinitoBack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordService _passwordService;
    public UsuarioController(AppDbContext context, IPasswordService passwordService)
    {
        _context = context;
        _passwordService= passwordService;
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
        Usuario? usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound($"No se econtro ningun usuario con el id {id}");
        }
        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioRegistroDto usuarioARegistrar)
    {
        bool existe = await _context.Usuarios
    .AnyAsync(usuario => usuario.Mail == usuarioARegistrar.Mail);
        if (existe)
        {
            return Conflict($"Ya existe un usuario con el correo {usuarioARegistrar.Mail}");
        }
        Usuario usuario = new Usuario();
        string hash = _passwordService.HashPassword(usuarioARegistrar.Pasword);

        usuario.Nombre = usuarioARegistrar.Nombre;
        usuario.Apellido = usuarioARegistrar.Apellido;
        usuario.Mail = usuarioARegistrar.Mail;
        usuario.PasswordHash = hash;
        usuario.Rol = "Usuario";
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
        return Ok("El usuario se registro correctamente");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> BorrarUsuarioPorId(int id)
    {
        Usuario? usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound($"No se econtro ningun usuario con el id {id}");
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return Ok($"El usuario con el id {id} fue eliminado con exito");
    }

    [HttpPut("{id}")]

    public async Task<IActionResult> EditarUsuario(int id, [FromBody] UsuarioEditarDto usuarioModificado)
    {
        Usuario? usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound($"El usuario con id {id} no existe");
        }
        usuario.Nombre = usuarioModificado.Nombre;
        usuario.Apellido = usuarioModificado.Apellido;
        usuario.Mail = usuarioModificado.Mail;

        await _context.SaveChangesAsync();
        return Ok($"El usuario con el id {id} fue modificado con exito ");
    }

}