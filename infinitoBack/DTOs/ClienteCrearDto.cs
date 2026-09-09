namespace infinitoBack.DTOs
{
    public class ClienteCrearDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Ci { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Direccion {get;set; }

    }
}
