using infinitoBack.Enum;

namespace infinitoBack.Utils
{
    public class Resultado
    {
        public bool Exitoso { get; }

        public string MensajeError { get; }

        public TipoError TipoErr { get; }

        protected Resultado(bool exitoso, string mensajeError, TipoError tipoErr)
        {
            Exitoso = exitoso;
            MensajeError = mensajeError;
            TipoErr = tipoErr;
        }

        public static Resultado Correcto() => new(true, string.Empty, TipoError.Ninguno);

        public static Resultado Error(string mensaje, TipoError tipoErr) => new(false, mensaje, tipoErr);
    }


}
