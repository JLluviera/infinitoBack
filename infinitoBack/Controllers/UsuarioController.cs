using infinitoBack.Data;
using infinitoBack.DTOs;
using infinitoBack.Interfaces;
using infinitoBack.Models;
using infinitoBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace infinitoBack.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    public UsuarioController(AppDbContext context, IPasswordService passwordService,ITokenService tokenService)
    {
        _context = context;
        _passwordService= passwordService;
        _tokenService = tokenService;
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
    [AllowAnonymous]
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
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDto)
    {
        Usuario? usuario = await _context.Usuarios
            .FirstOrDefaultAsync(usuario => usuario.Mail == usuarioLoginDto.Mail);

        if (usuario == null)
        {
            return Unauthorized("Correo o contraseña incorrectos.");
        }

        bool passwordCorrecta = _passwordService.VerificarPassword(
        usuarioLoginDto.Password,
        usuario.PasswordHash);

        if (!passwordCorrecta)
        {
            return Unauthorized("Correo o contraseña incorrectos.");
        }

        string token = _tokenService.GenerarToken(usuario);

        return Ok(new
        {
            Token = token
        });
    }

}