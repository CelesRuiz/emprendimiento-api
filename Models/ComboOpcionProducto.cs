namespace EmprendimientoApi.Models
{
    public class ComboOpcionProducto
    {
        public int ComboOpcionId { get; set; }
        public ComboOpcion? ComboOpcion { get; set; }
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
    }
}