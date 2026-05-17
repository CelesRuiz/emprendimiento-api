namespace EmprendimientoApi.Models
{
    public class Permiso : EntidadBase
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
    }
}