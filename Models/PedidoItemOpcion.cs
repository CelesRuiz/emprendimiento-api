namespace EmprendimientoApi.Models
{
    public class PedidoItemOpcion
    {
        public int Id { get; set; }
        public int PedidoItemId { get; set; }
        public PedidoItem? PedidoItem { get; set; }
        public int ComboOpcionId { get; set; }
        public ComboOpcion? ComboOpcion { get; set; }
        public int ProductoElegidoId { get; set; }
        public Producto? ProductoElegido { get; set; }
    }
}