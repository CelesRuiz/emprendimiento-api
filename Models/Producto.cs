using EmprendimientoApi.Models;
public class Producto: EntidadBase
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal PrecioVenta { get; set; }
    public int StockMinimo { get; set; }
    public ICollection<ProductoIngrediente> ProductoIngredientes { get; set; } = new List<ProductoIngrediente>();
}