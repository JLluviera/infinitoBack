namespace infinitoBack.DTOs
{
    public class PaginaDTO<T>
    {
        public List<T> Elementos { get; set; }
        public int? SiguienteCursor { get; set; }
        public bool HayMas {  get; set; }
    }
}
