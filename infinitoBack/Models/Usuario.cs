using System.ComponentModel.DataAnnotations;

namespace infinitoBack.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }
        
        public string Apellido { get; set; }

        public string Mail { get; set; }

        public string PasswordHash { get; set; }

        public string Rol { get; set; }

    }
}
