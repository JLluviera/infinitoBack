namespace infinitoBack.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        // Entidad afectada: Cliente, Reserva, Destino, etc.
        public string NombreEntidad { get; set; } = string.Empty;

        // Insert, Update, Delete
        public string Accion { get; set; } = string.Empty;

        // ID del registro afectado
        public string? ClavePrimaria { get; set; }

        // JSON con los cambios
        public string? Cambios { get; set; }

        // Usuario que realizó la acción
        public int? UsuarioId { get; set; }

        // Email del usuario
        public string? MailUsuario { get; set; }

        // IP desde donde se hizo
        public string? DireccionIp { get; set; }

        // Fecha y hora
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}