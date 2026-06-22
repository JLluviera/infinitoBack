using System.ComponentModel.DataAnnotations;

namespace infinitoBack.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; }
        
        public string ApellidoUsuario { get; set; }

        public string MailUsuario { get; set; }

        public string PasswordUsuario { get; set; }

        public string RolUsuario { get; set; }

    }
}
