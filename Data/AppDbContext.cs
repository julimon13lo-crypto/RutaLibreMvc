using Microsoft.EntityFrameworkCore;
using RutaLibreMvc.Models;

namespace RutaLibreMvc.Data
{
    /// <summary>
    /// Contexto conectado a la base de datos SQL Server "RutaLibreDB".
    /// Las tablas ya existen (se crean con los scripts de la carpeta "Base de datos"),
    /// así que aquí solo se indica cómo se llama cada tabla y columna.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Vehiculo> Vehiculos { get; set; } = null!;
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Renta> Rentas { get; set; } = null!;
        public DbSet<Pago> Pagos { get; set; } = null!;
        public DbSet<Empleado> Empleados { get; set; } = null!;
        public DbSet<Mantenimiento> Mantenimientos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- Categorias ----------
            modelBuilder.Entity<Categoria>(e =>
            {
                e.ToTable("Categorias");
                e.HasKey(c => c.CategoriaID);
                e.Property(c => c.CategoriaID).HasColumnName("CategoriaID");
                e.Property(c => c.NombreCategoria).HasMaxLength(50);
                e.Property(c => c.Descripcion).HasMaxLength(200);
            });

            // ---------- Vehiculos ----------
            modelBuilder.Entity<Vehiculo>(e =>
            {
                e.ToTable("Vehiculos");
                e.HasKey(v => v.Id);
                e.Property(v => v.Id).HasColumnName("VehiculoID");
                e.Property(v => v.Placa).HasMaxLength(10);
                e.Property(v => v.TipoVehiculo).HasMaxLength(10);
                e.Property(v => v.Marca).HasMaxLength(50);
                e.Property(v => v.Modelo).HasMaxLength(50);
                e.Property(v => v.Anio).HasColumnName("Año");
                e.Property(v => v.PrecioDia).HasColumnType("decimal(10,2)");
                e.Property(v => v.Transmision).HasMaxLength(20);
                e.Property(v => v.Estado).HasMaxLength(20);
                e.Property(v => v.CategoriaId).HasColumnName("CategoriaID");
                e.HasIndex(v => v.Placa).IsUnique();

                e.HasOne(v => v.Categoria)
                 .WithMany()
                 .HasForeignKey(v => v.CategoriaId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- Clientes ----------
            modelBuilder.Entity<Cliente>(e =>
            {
                e.ToTable("Clientes");
                e.HasKey(c => c.Id);
                e.Property(c => c.Id).HasColumnName("ClienteID");
                e.Property(c => c.NombreCompleto).HasMaxLength(100);
                e.Property(c => c.Cedula).HasMaxLength(20);
                e.Property(c => c.Telefono).HasMaxLength(20);
                e.Property(c => c.Email).HasMaxLength(100);
                e.HasIndex(c => c.Cedula).IsUnique();
            });

            // ---------- Empleados ----------
            modelBuilder.Entity<Empleado>(e =>
            {
                e.ToTable("Empleados");
                e.HasKey(x => x.EmpleadoID);
                e.Property(x => x.EmpleadoID).HasColumnName("EmpleadoID");
                e.Property(x => x.NombreCompleto).HasMaxLength(100);
                e.Property(x => x.Cedula).HasMaxLength(20);
                e.Property(x => x.Cargo).HasMaxLength(50);
                e.Property(x => x.Telefono).HasMaxLength(20);
                e.Property(x => x.Estado).HasMaxLength(20);
            });

            // ---------- Rentas ----------
            modelBuilder.Entity<Renta>(e =>
            {
                e.ToTable("Rentas");
                e.HasKey(r => r.Id);
                e.Property(r => r.Id).HasColumnName("RentaID");
                e.Property(r => r.VehiculoId).HasColumnName("VehiculoID");
                e.Property(r => r.ClienteId).HasColumnName("ClienteID");
                e.Property(r => r.EmpleadoId).HasColumnName("EmpleadoID");
                e.Property(r => r.TotalEstimado).HasColumnType("decimal(10,2)");
                e.Property(r => r.EstadoRenta).HasMaxLength(20);

                e.HasOne(r => r.Vehiculo).WithMany()
                 .HasForeignKey(r => r.VehiculoId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(r => r.Cliente).WithMany()
                 .HasForeignKey(r => r.ClienteId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(r => r.Empleado).WithMany()
                 .HasForeignKey(r => r.EmpleadoId).OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- Pagos ----------
            modelBuilder.Entity<Pago>(e =>
            {
                e.ToTable("Pagos");
                e.HasKey(p => p.PagoID);
                e.Property(p => p.PagoID).HasColumnName("PagoID");
                e.Property(p => p.RentaID).HasColumnName("RentaID");
                e.Property(p => p.Monto).HasColumnType("decimal(10,2)");
                e.Property(p => p.MetodoPago).HasMaxLength(30);

                e.HasOne(p => p.Renta).WithMany(r => r.Pagos)
                 .HasForeignKey(p => p.RentaID).OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- Mantenimientos ----------
            modelBuilder.Entity<Mantenimiento>(e =>
            {
                e.ToTable("Mantenimientos");
                e.HasKey(m => m.MantenimientoID);
                e.Property(m => m.MantenimientoID).HasColumnName("MantenimientoID");
                e.Property(m => m.VehiculoID).HasColumnName("VehiculoID");
                e.Property(m => m.TipoMantenimiento).HasMaxLength(50);
                e.Property(m => m.Descripcion).HasMaxLength(250);
                e.Property(m => m.Costo).HasColumnType("decimal(10,2)");

                e.HasOne(m => m.Vehiculo).WithMany()
                 .HasForeignKey(m => m.VehiculoID).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
