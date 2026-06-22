using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.ComponentModel.DataAnnotations;

namespace infinitoBack.Models
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }
        [Required]
        public string NombreCliente { get; set; }
        [Required]
        public string ApellidoCliente { get; set; }
        [Required]
        public int CiCliente { get; set; }
        [Required]
        public DateOnly FechaNacCliente { get; set; }
        public int TelCliente { get; set; }

        public float SaldoCliente { get; set; }
    }
}
