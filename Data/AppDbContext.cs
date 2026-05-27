using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<ProductoInsumo> ProductoInsumos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<MovimientoStock> MovimientosStock { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<RolPermiso> RolPermisos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboOpcion> ComboOpciones { get; set; }
        public DbSet<ComboOpcionProducto> ComboOpcionProductos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItems { get; set; }
        public DbSet<PedidoItemOpcion> PedidoItemOpciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductoInsumo>()
                .HasKey(pi => new { pi.ProductoId, pi.InsumoId });

            modelBuilder.Entity<Producto>()
                .Property(p => p.PrecioVenta)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Venta>()
                .Property(v => v.PrecioTotal)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProductoInsumo>()
                .Property(pi => pi.Cantidad)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Insumo>()
                .Property(i => i.PrecioPorUnidad)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RolPermiso>()
                .HasKey(rp => new { rp.RolId, rp.PermisoId });

            modelBuilder.Entity<Lote>()
                .Property(l => l.CantidadInicial)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Lote>()
                .Property(l => l.CantidadActual)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Producto>()
                .Property(p => p.CostoProduccion)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ComboOpcionProducto>()
                .HasKey(cop => new { cop.ComboOpcionId, cop.ProductoId });

            modelBuilder.Entity<Combo>()
                .Property(c => c.Precio)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PedidoItem>()
                .Property(pi => pi.PrecioUnitario)
                .HasColumnType("decimal(18,2)");
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
                    entidad.Entity.CreadoEn = DateTime.UtcNow;
                    entidad.Entity.ActualizadoEn = DateTime.UtcNow;
                }
                if (entidad.State == EntityState.Modified)
                {
                    entidad.Entity.ActualizadoEn = DateTime.UtcNow;
                }
            }
        }
    }
}