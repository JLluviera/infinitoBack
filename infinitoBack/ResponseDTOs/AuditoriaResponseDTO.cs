namespace infinitoBack.ResponseDTOs
{
    public class AuditoriaResponseDTO
    {
        public int Id { get; set; }

        public string NombreEntidad { get; set; } = string.Empty;

        public string Accion { get; set; } = string.Empty;

        public string? ClavePrimaria { get; set; }

        public string? Cambios { get; set; }

        public int? UsuarioId { get; set; }

        public string? MailUsuario { get; set; }

        public string? DireccionIp { get; set; }

        public DateTime Timestamp { get; set; }
    }
}