using infinitoBack.Models;
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
        public int IdPais { get; set; }
        [Required]
        public string Descripcion { get; set; }
    }
}
