namespace EmprendimientoApi.Models
{
    public class RecetaIngrediente
    {
        public int RecetaId { get; set; }
        public Receta? Receta { get; set; } = null!;
        public int IngredienteId { get; set; }
        public Ingrediente? Ingrediente { get; set; } = null!;
        public decimal CantidadGramos { get; set; }
    }
}