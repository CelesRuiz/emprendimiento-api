namespace EmprendimientoApi.Models
{
    public class Insumo : EntidadBase
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioPorUnidad { get; set; }
        public UnidadMedida UnidadMedida { get; set; }
        public int StockMinimo { get; set; }
    }
}