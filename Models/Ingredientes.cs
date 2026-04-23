namespace EmprendimientoApi.Models
{
    public class Ingrediente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioPorKilo { get; set; }
    }
}