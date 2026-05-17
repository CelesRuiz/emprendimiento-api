using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Ingrediente> Ingredientes { get; set; }
        public DbSet<ProductoIngrediente> ProductoIngredientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<MovimientoStock> MovimientosStock { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<RolPermiso> RolPermisos { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductoIngrediente>()
                .HasKey(pi => new { pi.ProductoId, pi.IngredienteId });

            modelBuilder.Entity<Producto>()
                .Property(p => p.PrecioVenta)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Venta>()
                .Property(v => v.PrecioTotal)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProductoIngrediente>()
                .Property(pi => pi.Cantidad)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Ingrediente>()
                .Property(i => i.PrecioPorKilo)
                .HasColumnType("decimal(18,2)"); 

            modelBuilder.Entity<RolPermiso>()
                .HasKey(rp => new { rp.RolId, rp.PermisoId });   
        }

        public override int SaveChanges()
        {
            ActualizarFechas();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ActualizarFechas();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ActualizarFechas()
        {
            var entidades = ChangeTracker.Entries<EntidadBase>();
            foreach (var entidad in entidades)
            {
                if (entidad.State == EntityState.Added)
                {
                    entidad.Entity.CreadoEn = DateTime.Now;
                    entidad.Entity.ActualizadoEn = DateTime.Now;
                }
                if (entidad.State == EntityState.Modified)
                {
                    entidad.Entity.ActualizadoEn = DateTime.Now;
                }
            }
        }
    }
}