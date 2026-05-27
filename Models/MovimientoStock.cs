namespace EmprendimientoApi.Models
{
    public enum TipoMovimiento
    {
        Entrada,
        Salida
    }

    public enum MotivoSalida
    {
        Venta,
        Vencimiento,
        Descarte
    }

    public class MovimientoStock : EntidadBase
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public TipoMovimiento Tipo { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public MotivoSalida? MotivoSalida { get; set; }
    }
}