namespace EmprendimientoApi.Models
{
    public class PedidoItem : EntidadBase
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
        public int? ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public int? ComboId { get; set; }
        public Combo? Combo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public ICollection<PedidoItemOpcion> OpcionesElegidas { get; set; } = new List<PedidoItemOpcion>();
    }
}