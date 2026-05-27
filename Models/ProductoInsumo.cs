namespace EmprendimientoApi.Models
{
    public class ProductoInsumo
    {
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public int InsumoId { get; set; }
        public Insumo? Insumo { get; set; }
        public decimal Cantidad { get; set; }
    }
}