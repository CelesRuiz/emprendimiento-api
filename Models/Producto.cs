using EmprendimientoApi.Models;

namespace EmprendimientoApi.Models
{
    public class Producto : EntidadBase
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; }
        public int StockMinimo { get; set; }
        public bool EsCombo { get; set; } = false;
        public int DiasMaxFrescura { get; set; } = 0;
        public ICollection<ProductoInsumo> ProductoInsumos { get; set; } = new List<ProductoInsumo>();
        public decimal CostoProduccion { get; set; } = 0;
    }
}