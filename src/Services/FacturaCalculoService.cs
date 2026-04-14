using src.Models;

namespace src.Services
{
    /* Clase que contiene todos los cálculos matemáticos y la lógica de las facturas,
     * calculando el importe de cada línea y los importes de la cabecera*/
    public static class FacturaCalculoService
    {
        // Calcular el importe de cada línea
        public static decimal CalcularImpLinea(decimal cantidad, decimal precio, decimal dto, decimal dtoProntoPago)
        {
            var impBruto = cantidad * precio;
            var impTrasDtoLinea = impBruto * (1 - dto / 100);
            var impTrasDtoPP = impTrasDtoLinea * (1 - dtoProntoPago / 100);
            return Math.Round(impTrasDtoPP, 8);
        }

        public static void CalcularCabecera(
            FacturaVentaCabecera cabecera,
            decimal dtoFactura,
            decimal dtoProntoPago,
            decimal recFinan,
            decimal retencionIRPF,
            Dictionary<string, decimal>tiposIvaPorLinea,
            decimal porcentajeRE = 0)
        {
            // Calcular importe por líneas
            cabecera.ImpLineas = cabecera.Lineas.Sum(l => l.Importe);

            // Calcular descuento de la factura sobre el total de líneas
            cabecera.DtoFactura = dtoFactura;
            cabecera.ImpDtoFactura = Math.Round(cabecera.ImpLineas * dtoFactura / 100, 8);

            // Calcular descuento por pronto pago
            cabecera.DtoProntoPago = dtoProntoPago;
            cabecera.ImpDpp = Math.Round((cabecera.ImpLineas - cabecera.ImpDtoFactura) * dtoProntoPago / 100, 8);

            // Calcular el recargo financiero
            cabecera.RecFinan = recFinan;
            cabecera.ImpRecFinan = Math.Round((cabecera.ImpLineas - cabecera.ImpDtoFactura) * recFinan / 100, 8);

            // Base imponible
            cabecera.BaseImponible = Math.Round(cabecera.ImpLineas - cabecera.ImpDtoFactura - cabecera.ImpDpp + cabecera.ImpRecFinan, 8);

            // Cáculo del IVA proporcional
            cabecera.ImpIva = CalcularIvaProporcionalVenta(cabecera, tiposIvaPorLinea);

            // Cálculo del recargo de equivalencia y retención IRPF
            cabecera.ImpRE = Math.Round(cabecera.BaseImponible * porcentajeRE / 100, 8);
            cabecera.ImpRetencion = Math.Round(cabecera.BaseImponible * retencionIRPF / 100 , 8);

            // Cálculo del importe total final
            cabecera.ImpTotal = Math.Round(cabecera.BaseImponible + cabecera.ImpIva + cabecera.ImpRE - cabecera.ImpRetencion, 8);
        }

        public static void CalcularCabecera(
            FacturaCompraCabecera cabecera,
            decimal dtoFactura,
            decimal dtoProntoPago,
            decimal recFinan,
            decimal retencionIRPF,
            Dictionary<string, decimal> tiposIvaPorLinea,
            decimal porcentajeRE = 0)
        {
            // Calcular importe por líneas
            cabecera.ImpLineas = cabecera.Lineas.Sum(l => l.Importe);

            // Calcular descuento de la factura sobre el total de líneas
            cabecera.DtoFactura = dtoFactura;
            cabecera.ImpDtoFactura = Math.Round(cabecera.ImpLineas * dtoFactura / 100, 8);

            // Calcular descuento por pronto pago
            cabecera.DtoProntoPago = dtoProntoPago;
            cabecera.ImpDpp = Math.Round((cabecera.ImpLineas - cabecera.ImpDtoFactura) * dtoProntoPago / 100, 8);

            // Calcular el recargo financiero
            cabecera.RecFinan = recFinan;
            cabecera.ImpRecFinan = Math.Round((cabecera.ImpLineas - cabecera.ImpDtoFactura) * recFinan / 100, 8);

            // Base imponible
            cabecera.BaseImponible = Math.Round(cabecera.ImpLineas - cabecera.ImpDtoFactura - cabecera.ImpDpp + cabecera.ImpRecFinan, 8);

            // Cáculo del IVA proporcional
            cabecera.ImpIva = CalcularIvaProporcionalCompra(cabecera, tiposIvaPorLinea);

            // Cálculo del recargo de equivalencia y retención IRPF
            cabecera.ImpRE = Math.Round(cabecera.BaseImponible * porcentajeRE / 100, 8);
            cabecera.ImpRetencion = Math.Round(cabecera.BaseImponible * retencionIRPF / 100, 8);

            // Cálculo del importe total final
            cabecera.ImpTotal = Math.Round(cabecera.BaseImponible + cabecera.ImpIva + cabecera.ImpRE - cabecera.ImpRetencion, 8);
        }

        // Método para calcular el IVA proporcional en las facturas de venta
        private static decimal CalcularIvaProporcionalVenta(
            FacturaVentaCabecera cabecera,
            Dictionary<string, decimal> tiposIva)
        {
            if (cabecera.ImpLineas == 0) return 0;

            decimal totalIva = 0;
            foreach (var linea in cabecera.Lineas)
            {
                // Peso de esta línea sobre el total
                var proporcion = linea.Importe / cabecera.ImpLineas;
                var baseLinea = cabecera.BaseImponible * proporcion;

                // Buscar el % IVA de esta línea (0 si no tiene tipo IVA)
                var pctIva = ObtenerPorcentajeIva(linea.TipoIva, tiposIva);
                totalIva += Math.Round(baseLinea * pctIva / 100, 8);
            }
            return Math.Round(totalIva, 8);
        }

        // Método para calcular el IVA proporcional en las facturas de compra
        private static decimal CalcularIvaProporcionalCompra(
            FacturaCompraCabecera cabecera,
            Dictionary<string, decimal> tiposIva)
        {
            if (cabecera.ImpLineas == 0) return 0;

            decimal totalIva = 0;
            foreach (var linea in cabecera.Lineas)
            {
                // Peso de esta línea sobre el total
                var proporcion = linea.Importe / cabecera.ImpLineas;
                var baseLinea = cabecera.BaseImponible * proporcion;

                // Buscar el % IVA de esta línea (0 si no tiene tipo IVA)
                var pctIva = ObtenerPorcentajeIva(linea.TipoIva, tiposIva);
                totalIva += Math.Round(baseLinea * pctIva / 100, 8);
            }
            return Math.Round(totalIva, 8);
        }

        // Método para obtener el porcentaje de IVA correspondiente
        private static decimal ObtenerPorcentajeIva(string? tipoIva, Dictionary<string, decimal> tiposIva)
        {
            if (string.IsNullOrEmpty(tipoIva)) return 0;
            return tiposIva.TryGetValue(tipoIva, out var pct) ? pct : 0;
        }
    }
}
