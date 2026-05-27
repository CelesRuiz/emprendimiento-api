namespace EmprendimientoApi.Models
{
    public class Lote : EntidadBase
    {
        public int Id { get; set; }
        public int InsumoId { get; set; }
        public Insumo? Insumo { get; set; }
        public decimal CantidadInicial { get; set; }
        public decimal CantidadActual { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool Cerrado { get; set; } = false;
        public string? MotivoCierre { get; set; }
    }
}