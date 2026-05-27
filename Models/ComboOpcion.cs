namespace EmprendimientoApi.Models
{
    public class ComboOpcion : EntidadBase
    {
        public int Id { get; set; }
        public int ComboId { get; set; }
        public Combo? Combo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool EsEleccion { get; set; } = true;
        public int? ProductoFijoId { get; set; }
        public Producto? ProductoFijo { get; set; }
        public ICollection<ComboOpcionProducto> Productos { get; set; } = new List<ComboOpcionProducto>();
    }
}