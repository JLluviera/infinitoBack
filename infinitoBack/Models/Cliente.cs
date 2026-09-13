using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.ComponentModel.DataAnnotations;

namespace infinitoBack.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public int Ci { get; set; }

        public DateOnly? FechaVencimientoCi { get; set; }

        [Required]
        public DateOnly FechaNacimiento { get; set; }
        public string Telefono { get; set; }

        public List<Reserva>? Reservas { get; set; }

        public List<Reserva>? ReservasPagas { get; set; }
    }
}
