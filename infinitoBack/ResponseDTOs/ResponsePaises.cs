using infinitoBack.Models;
using infinitoBack.DTOs;

namespace infinitoBack.ResponseDTOs
{
    public class ResponsePaises
    {
        public int Id { get; set; }
        public string NombrePais { get; set; }

        public string CodigoPais { get; set; }

        public List<DestinoCrearDto> Destinos { get; set; }
    }
}