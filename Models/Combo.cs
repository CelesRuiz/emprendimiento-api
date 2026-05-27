namespace EmprendimientoApi.Models
{
    public class Combo : EntidadBase
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public bool EsActivo { get; set; } = true;
        public ICollection<ComboOpcion> Opciones { get; set; } = new List<ComboOpcion>();
    }
}