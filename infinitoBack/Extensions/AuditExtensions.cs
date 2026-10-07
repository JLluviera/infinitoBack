using Audit.Core;
using infinitoBack.Models;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Builder
{
    public static class AuditExtensions
    {
        public static IApplicationBuilder UseAuditConfiguration(this IApplicationBuilder app)
        {
            IHttpContextAccessor httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();

            Audit.Core.Configuration.Setup()
                .UseEntityFramework(config => config
                    .UseDbContext<infinitoBack.Data.AuditDbContext>()
                    .DisposeDbContext()
                    .AuditTypeMapper(_ => typeof(AuditLog))
                    .AuditEntityAction<AuditLog>((evento, entrada, auditoria) =>
                    {
                        auditoria.NombreEntidad = entrada.EntityType.Name;
                        auditoria.Accion = entrada.Action;

                        if (entrada.PrimaryKey.Count > 0)
                        {
                            auditoria.ClavePrimaria =
                                entrada.PrimaryKey.First().Value?.ToString();
                        }

                        auditoria.Cambios = entrada.ToJson();
                        auditoria.Timestamp = evento.StartDate;

                        HttpContext? contexto = httpContextAccessor.HttpContext;

                        if (contexto != null)
                        {
                            Claim? claimUsuario = contexto.User.FindFirst(
                                ClaimTypes.NameIdentifier
                            );

                            Claim? claimMail = contexto.User.FindFirst(
                                ClaimTypes.Email
                            );

                            if (claimUsuario != null &&
                                int.TryParse(claimUsuario.Value, out int usuarioId))
                            {
                                auditoria.UsuarioId = usuarioId;
                            }

                            if (claimMail != null)
                            {
                                auditoria.MailUsuario = claimMail.Value;
                            }

                            auditoria.DireccionIp =
                                contexto.Connection.RemoteIpAddress?.ToString();
                        }
                    })
                    .IgnoreMatchedProperties(true)
                );
            return app;
        }
    }
}
