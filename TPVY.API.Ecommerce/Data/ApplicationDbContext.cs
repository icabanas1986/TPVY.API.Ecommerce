using Microsoft.EntityFrameworkCore;
using TPVY.API.Ecommerce.Models;
using TPVY.API.Ecommerce.Models.Auth;

namespace TPVY.API.Ecommerce.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public ApplicationDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configura la conexión manualmente solo en modo diseño
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relaciones
            modelBuilder.Entity<Rol>()
                .HasMany(r => r.Usuarios)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.RolId);

            // Datos iniciales
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador" },
                new Rol { Id = 2, Nombre = "Vendedor" }
            );

            modelBuilder.Entity<Categoria>()
        .HasMany(c => c.Productos)
        .WithOne(p => p.Categoria)
        .HasForeignKey(p => p.CategoriaId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Categoria>().HasData(
        new Categoria { Id = 1, Nombre = "Lectores de Código de Barras" },
        new Categoria { Id = 2, Nombre = "Impresoras Térmicas" },
        new Categoria { Id = 3, Nombre = "Cajas Registradoras" }
    );
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalle> PedidoDetalles { get; set; }

        public DbSet<UsuariosAuth> UsuariosAuth { get; set; }
        public DbSet<Rol> Roles { get; set; }

    }
}
