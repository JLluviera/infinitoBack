using System.ComponentModel.DataAnnotations;

namespace infinitoBack.DTOs
{
    public class DestinoCrearDto
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Ciudad { get; set; }
        [Required]
        public string Pais { get; set; }
        [Required]
        public string Descripcion { get; set; }
    }
}
