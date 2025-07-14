namespace LavacarSeLoChaneoADomicilio.Models
{
    public class Lavado
    {
        public int Id { get; set; }
        public string PlacaVehiculo { get; set; } = string.Empty;
        public string IdCliente { get; set; } = string.Empty;
        public string IdEmpleado { get; set; } = string.Empty;
        public TipoLavado Tipo { get; set; }
        public EstadoLavado Estado { get; set; }
        public decimal Precio { get; set; }
        public string Detalle { get; set; } = string.Empty;

        public decimal IVA => Math.Round(Precio * 0.13M, 2);
        public decimal Total => Precio + IVA;
    }
}
