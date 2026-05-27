namespace EmprendimientoApi.Models
{
    public class Pedido : EntidadBase
    {
        public int Id { get; set; }
        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;
        public DateTime FechaPedido { get; set; }
        public string? OrigenPedido { get; set; } // "PedidosYa", "MercadoPago", "Manual"
        public string? Idexterno { get; set; } // ID del pedido en PedidosYa o MercadoPago
        public ICollection<PedidoItem> Items { get; set; } = new List<PedidoItem>();
    }
}