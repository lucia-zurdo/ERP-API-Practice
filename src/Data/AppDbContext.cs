using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<Articulos> Articulos { get; set; }
        public DbSet<FacturaCompraCabecera> FacturasCompraCabecera { get; set; }
        public DbSet<FacturaCompraLinea> FacturasCompraLinea { get; set; }
        public DbSet<FacturaVentaCabecera> FacturasVentaCabecera { get; set; }
        public DbSet<FacturaVentaLinea> FacturasVentaLinea { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* Config. precisión de decimales requerida en las columnas (23, 8).
             * Por defecto EF Core utilizaría decimal(18, 2).
             * En tablas de líneas de Factura, se incluye config. de las relaciones cabecera ↔ línea */

            // Tabla Clientes
            modelBuilder.Entity<Clientes>().Property(c => c.DtoComercial).HasColumnType("numeric(23,8)");

            // Tabla Proveedores
            modelBuilder.Entity<Proveedores>().Property(p => p.DtoComercial).HasColumnType("numeric(23,8)");

            // Tabla Artículos
            modelBuilder.Entity<Articulos>(entity =>
            {
                entity.Property(a => a.PesoNeto).HasColumnType("numeric(23,8)");
                entity.Property(a => a.PesoBruto).HasColumnType("numeric(23,8)");
                entity.Property(a => a.Plazo).HasColumnType("numeric(23,8)");
                entity.Property(a => a.Volumen).HasColumnType("numeric(23,8)");
            });

            // Tabla FacturaCompraCabecera
            modelBuilder.Entity<FacturaCompraCabecera>(entity =>
            {
                entity.Property(f => f.ImpLineas).HasColumnType("numeric(23,8)");
                entity.Property(f => f.DtoFactura).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpDtoFactura).HasColumnType("numeric(23,8)");
                entity.Property(f => f.DtoProntoPago).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpDpp).HasColumnType("numeric(23,8)");
                entity.Property(f => f.RecFinan).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpRecFinan).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpRE).HasColumnType("numeric(23,8)");
                entity.Property(f => f.BaseImponible).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpIva).HasColumnType("numeric(23,8)");
                entity.Property(f => f.RetencionIRPF).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpRetencion).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpTotal).HasColumnType("numeric(23,8)");
            });

            // Tabla FacturaCompraLinea
            modelBuilder.Entity<FacturaCompraLinea>(entity =>
            {
                // Config. decimales
                entity.Property(f => f.Cantidad).HasColumnType("numeric(23,8)");
                entity.Property(f => f.Precio).HasColumnType("numeric(23,8)");
                entity.Property(f => f.Dto).HasColumnType("numeric(23,8)");
                entity.Property(f => f.DtoProntoPago).HasColumnType("numeric(23,8)");
                entity.Property(f => f.Importe).HasColumnType("numeric(23,8)");

                // Config. relaciones
                entity.HasOne(linea => linea.Cabecera)
                .WithMany(cab => cab.Lineas)
                .HasForeignKey(linea => linea.IdFactura)
                .OnDelete(DeleteBehavior.Cascade); // En cascada
            });

            // Tabla FacturaVentaCabecera
            modelBuilder.Entity<FacturaVentaCabecera>(entity =>
            {
                entity.Property(f => f.ImpLineas).HasColumnType("numeric(23,8)");
                entity.Property(f => f.DtoFactura).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpDtoFactura).HasColumnType("numeric(23,8)");
                entity.Property(f => f.DtoProntoPago).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpDpp).HasColumnType("numeric(23,8)");
                entity.Property(f => f.RecFinan).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpRecFinan).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpRE).HasColumnType("numeric(23,8)");
                entity.Property(f => f.BaseImponible).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpIva).HasColumnType("numeric(23,8)");
                entity.Property(f => f.RetencionIRPF).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpRetencion).HasColumnType("numeric(23,8)");
                entity.Property(f => f.ImpTotal).HasColumnType("numeric(23,8)");
            });

            // Tabla FacturaVentaLinea
            modelBuilder.Entity<FacturaVentaLinea>(entity =>
            {
                entity.Property(f => f.Cantidad).HasColumnType("numeric(23,8)");
                entity.Property(f => f.Precio).HasColumnType("numeric(23,8)");
                entity.Property(f => f.Dto).HasColumnType("numeric(23,8)");
                entity.Property(f => f.DtoProntoPago).HasColumnType("numeric(23,8)");
                entity.Property(f => f.Importe).HasColumnType("numeric(23,8)");

                // Config. relaciones
                entity.HasOne(linea => linea.Cabecera)
                .WithMany(cab => cab.Lineas)
                .HasForeignKey(linea => linea.IdFactura)
                .OnDelete(DeleteBehavior.Cascade); // En cascada
            });

        }
    }
}
