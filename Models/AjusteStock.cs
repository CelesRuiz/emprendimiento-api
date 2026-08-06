namespace EmprendimientoApi.Models
{
    public class AjusteStock : EntidadBase
    {
        public int Id { get; set; }
        public int? InsumoId { get; set; }
        public Insumo? Insumo { get; set; }
        public int? ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public decimal CantidadAjuste { get; set; }
        public decimal DiferenciaDinero { get; set; }
        public DateTime Fecha { get; set; }
        public string? Motivo { get; set; }
    }
}