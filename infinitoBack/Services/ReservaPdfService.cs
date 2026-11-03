
using infinitoBack.Data;
using infinitoBack.Enum;
using infinitoBack.Models;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace infinitoBack.Services
{
    public class ReservaPdfService : IReservaPdfService
    {
        private readonly AppDbContext _context;

        public ReservaPdfService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GenerarPdfAsync(int idReserva)
        {
            var reserva = await _context.Reservas
                .AsNoTracking()
                .Include(r => r.ClientePagador)
                .Include(r => r.Excursion)
                    .ThenInclude(e => e.Destino)
                .Include(r => r.Paquete)
                .Include(r => r.ClientesIncluidos)
                .Include(r => r.Transacciones)
                .FirstOrDefaultAsync(r => r.Id == idReserva);

            if (reserva == null)
                throw new KeyNotFoundException(
                    $"No existe la reserva con ID {idReserva}.");

            // Mantenemos la regla de cuenta corriente:
            // solo Pago y UsoDeSaldo reducen la deuda de la reserva.
            decimal totalPagado = reserva.Transacciones
                .Where(t =>
                    t.Estado == EstadoTransaccion.Pago ||
                    t.Estado == EstadoTransaccion.UsoDeSaldo)
                .Sum(t => t.Monto);

            decimal saldoPendiente = Math.Max(
                0,
                reserva.MontoTotal - totalPagado);

            var pdf = Document.Create(documento =>
            {
                documento.Page(pagina =>
                {
                    pagina.Size(PageSizes.A4);
                    pagina.Margin(35);
                    pagina.DefaultTextStyle(estilo => estilo.FontSize(10));

                    pagina.Header().Column(columna =>
                    {
                        columna.Item()
                            .Text("INFINITO VIAJES")
                            .FontSize(22)
                            .Bold()
                            .FontColor(Colors.Blue.Medium);

                        columna.Item()
                            .Text($"Comprobante de reserva N.º {reserva.Id}")
                            .FontSize(14)
                            .Bold();

                        columna.Item()
                            .Text($"Fecha de reserva: {reserva.FechaReserva:dd/MM/yyyy}");
                    });

                    pagina.Content().PaddingVertical(20).Column(columna =>
                    {
                        columna.Spacing(8);

                        columna.Item().Text("DATOS DE LA RESERVA").Bold();
                        columna.Item().Text($"Estado: {reserva.EstadoReserva}");

                        columna.Item().Text("TITULAR").Bold();
                        columna.Item().Text(
                            $"Nombre: {reserva.ClientePagador.Nombre} " +
                            $"{reserva.ClientePagador.Apellido}");
                        columna.Item().Text(
                            $"Cédula: {reserva.ClientePagador.Ci}");
                        columna.Item().Text(
                            $"Teléfono: {reserva.ClientePagador.Telefono ?? "No registrado"}");

                        columna.Item().PaddingTop(8)
                            .Text("DATOS DEL VIAJE").Bold();

                        columna.Item().Text(
                            $"Excursión: {reserva.Excursion.Nombre}");
                        columna.Item().Text(
                            $"Destino: {reserva.Excursion.Destino.Nombre}");
                        columna.Item().Text(
                            $"Salida: {reserva.Excursion.FechaSalida:dd/MM/yyyy}");
                        columna.Item().Text(
                            $"Duración: {reserva.Excursion.CantDias} días");
                        columna.Item().Text(
                            $"Paquete: {reserva.Paquete.Nombre}");
                        columna.Item().Text(
                            $"Descripción: {reserva.Paquete.Descripcion}");

                        columna.Item().PaddingTop(8)
                            .Text("PASAJEROS INCLUIDOS").Bold();

                        var pasajeros = reserva.ClientesIncluidos
                            .Append(reserva.ClientePagador)
                            .DistinctBy(c => c.Id)
                            .ToList();

                        if (pasajeros.Count == 0)
                        {
                            columna.Item().Text("No hay pasajeros registrados.");
                        }
                        else
                        {
                            columna.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columnas =>
                                {
                                    columnas.RelativeColumn(3);
                                    columnas.RelativeColumn(2);
                                });

                                tabla.Header(cabecera =>
                                {
                                    cabecera.Cell().Element(CeldaEncabezado)
                                        .Text("Nombre");
                                    cabecera.Cell().Element(CeldaEncabezado)
                                        .Text("Cédula");
                                });

                                foreach (var pasajero in pasajeros)
                                {
                                    tabla.Cell().Element(Celda)
                                        .Text($"{pasajero.Nombre} {pasajero.Apellido}");
                                    tabla.Cell().Element(Celda)
                                        .Text(pasajero.Ci.ToString());
                                }
                            });
                        }

                        columna.Item().PaddingTop(8)
                            .Text("RESUMEN ECONÓMICO").Bold();

                        columna.Item().Text(
                            $"Monto total: {reserva.MontoTotal:N2}");
                        columna.Item().Text(
                            $"Total pagado: {totalPagado:N2}");
                        columna.Item().Text(
                            $"Saldo pendiente: {saldoPendiente:N2}");

                        columna.Item().PaddingTop(8)
                            .Text("MOVIMIENTOS REGISTRADOS").Bold();

                        if (reserva.Transacciones.Count == 0)
                        {
                            columna.Item().Text(
                                "Todavía no hay transacciones registradas.");
                        }
                        else
                        {
                            columna.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columnas =>
                                {
                                    columnas.RelativeColumn(2);
                                    columnas.RelativeColumn(2);
                                    columnas.RelativeColumn(2);
                                    columnas.RelativeColumn(3);
                                });

                                tabla.Header(cabecera =>
                                {
                                    cabecera.Cell().Element(CeldaEncabezado)
                                        .Text("Fecha");
                                    cabecera.Cell().Element(CeldaEncabezado)
                                        .Text("Estado");
                                    cabecera.Cell().Element(CeldaEncabezado)
                                        .Text("Monto");
                                    cabecera.Cell().Element(CeldaEncabezado)
                                        .Text("Forma de pago");
                                });

                                foreach (var transaccion in reserva.Transacciones)
                                {
                                    tabla.Cell().Element(Celda)
                                        .Text(transaccion.FechaCreacion.ToString("dd/MM/yyyy"));
                                    tabla.Cell().Element(Celda)
                                        .Text(transaccion.Estado.ToString());
                                    tabla.Cell().Element(Celda)
                                        .Text(transaccion.Monto.ToString("N2"));
                                    tabla.Cell().Element(Celda)
                                        .Text(transaccion.FormaDePago.ToString());
                                }
                            });
                        }
                    });

                    pagina.Footer().AlignCenter().Text(
                        "Infinito Viajes - Comprobante generado por el sistema");
                });
            });

            return pdf.GeneratePdf();
        }

        private static IContainer Celda(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(5)
                .PaddingHorizontal(3);
        }

        private static IContainer CeldaEncabezado(IContainer container)
        {
            return container
                .Background(Colors.Grey.Lighten3)
                .BorderBottom(1)
                .Padding(5);
        }
    }
}
