using System.Text.Json.Serialization;

namespace infinitoBack.Enum
{
    public enum EstadoTransaccion
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        Pago,
        CreditoPorAnulacion,
        UsoDeSaldo,
        DevolucionPago
    }
}
