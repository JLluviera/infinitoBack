using System.Text.Json.Serialization;

namespace infinitoBack.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EstadoRes
    {
        Pendiente,
        Confirmada,
        Cancelada,
        Anulada
    }
}
