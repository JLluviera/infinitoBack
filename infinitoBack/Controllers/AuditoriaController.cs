using infinitoBack.Data;
using infinitoBack.DTOs;
using infinitoBack.ResponseDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace infinitoBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriaController : ControllerBase
    {
        private readonly AuditDbContext _context;

        public AuditoriaController(AuditDbContext context)
        {
            _context = context;
        }

        [HttpGet("paginado")]
        public async Task<IActionResult> ObtenerAuditoriasPaginadas(int? afterId)
        {
            int cantidad = 10;
            int ultimoId = afterId ?? 0;

            List<AuditoriaResponseDTO> auditorias = await _context.AuditLogs
                .Where(auditoria => auditoria.Id > ultimoId)
                .OrderBy(auditoria => auditoria.Id)
                .Select(auditoria => new AuditoriaResponseDTO
                {
                    Id = auditoria.Id,
                    NombreEntidad = auditoria.NombreEntidad,
                    Accion = auditoria.Accion,
                    ClavePrimaria = auditoria.ClavePrimaria,
                    Cambios = auditoria.Cambios,
                    UsuarioId = auditoria.UsuarioId,
                    MailUsuario = auditoria.MailUsuario,
                    DireccionIp = auditoria.DireccionIp,
                    Timestamp = auditoria.Timestamp
                })
                .Take(cantidad + 1)
                .ToListAsync();

            bool hayMas = auditorias.Count > cantidad;

            if (hayMas)
            {
                auditorias.RemoveAt(auditorias.Count - 1);
            }

            int? siguienteCursor = null;

            if (auditorias.Count > 0)
            {
                siguienteCursor = auditorias[^1].Id;
            }

            PaginaDTO<AuditoriaResponseDTO> pagina =
                new PaginaDTO<AuditoriaResponseDTO>
                {
                    Elementos = auditorias,
                    SiguienteCursor = siguienteCursor,
                    HayMas = hayMas
                };

            return Ok(pagina);
        }
    }
}