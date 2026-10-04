using System.Text.Json.Serialization;

namespace infinitoBack.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoAsiento
    {
        Standard,

        Cama,

        Vacio
    }
}
