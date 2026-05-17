namespace EmprendimientoApi.Models
{
    public class Rol : EntidadBase
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
    }
}