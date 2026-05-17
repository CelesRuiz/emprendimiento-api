namespace EmprendimientoApi.Models
{
    public class ProductoIngrediente
    {
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public int IngredienteId { get; set; }
        public Ingrediente? Ingrediente { get; set; }
        public decimal Cantidad { get; set; }
    }
}