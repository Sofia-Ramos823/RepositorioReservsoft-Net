//using Microsoft.EntityFrameworkCore;
//using reservsoft.Models;

//namespace reservsoft.DATA
//{
//    public class ApplicationDbContext : DbContext
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

//        public DbSet<Apartamentos> Apartamentos { get; set; }
//        public DbSet<Descuento> Descuentos { get; set; }
//        public DbSet<Reservas> Reservas { get; set; }
//        public DbSet<Mobiliario> Mobiliarios { get; set; }
//        public DbSet<Usuario> Usuarios { get; set; } // Incluye la entidad Usuario para el login.

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // Configuración de precisión y escala para campos de tipo decimal
//            modelBuilder.Entity<Descuento>()
//                .Property(d => d.Precio)
//                .HasColumnType("decimal(18,2)");

//            // Relación entre Apartamentos y Descuentos
//            modelBuilder.Entity<Descuento>()
//                .HasOne<Apartamentos>()
//                .WithMany()
//                .HasForeignKey(d => d.IdApartamento)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<Mobiliario>()
//                .HasIndex(m => m.IdentMobiliario)
//                .IsUnique()
//                .HasDatabaseName("IX_IdentMobiliario_Unique");

//            // Relación entre Apartamentos y Mobiliarios
//            modelBuilder.Entity<Mobiliario>()
//                .HasOne<Apartamentos>()
//                .WithMany()
//                .HasForeignKey(m => m.IdApartamento)
//                .OnDelete(DeleteBehavior.Cascade);

//            // Convertir el Enum Estado a string en la base de datos
//            modelBuilder.Entity<Mobiliario>()
//                .Property(m => m.Estado)
//                .HasConversion<string>(); // Almacenar como texto

//            modelBuilder.Entity<Reservas>()
//                .HasMany(r => r.Apartamentos)
//                .WithMany(a => a.Reserva)
//                .UsingEntity(j => j.ToTable("ReservasApartamentos"));

//            // Configuración específica para Usuario
//            modelBuilder.Entity<Usuario>()
//                .HasIndex(u => u.Email) // Asegurar que el correo sea único
//                .IsUnique();
//        }
//    }
//}


using Microsoft.EntityFrameworkCore;
using reservsoft.Models;

namespace reservsoft.DATA
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Apartamentos> Apartamentos { get; set; }
        public DbSet<Descuento> Descuentos { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<Mobiliario> Mobiliarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; } // Incluye la entidad Usuario para el login.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de precisión y escala para campos de tipo decimal
            modelBuilder.Entity<Descuento>()
                .Property(d => d.Precio)
                .HasColumnType("decimal(18,2)");

            // Relación entre Apartamentos y Descuentos
            modelBuilder.Entity<Descuento>()
                .HasOne<Apartamentos>()
                .WithMany()
                .HasForeignKey(d => d.IdApartamento)
                .OnDelete(DeleteBehavior.Restrict); // Bloquear eliminación si hay descuentos asociados

            modelBuilder.Entity<Mobiliario>()
                .HasIndex(m => m.IdentMobiliario)
                .IsUnique()
                .HasDatabaseName("IX_IdentMobiliario_Unique");

            // Relación entre Apartamentos y Mobiliarios
            modelBuilder.Entity<Mobiliario>()
                .HasOne<Apartamentos>()
                .WithMany()
                .HasForeignKey(m => m.IdApartamento)
                .OnDelete(DeleteBehavior.Restrict); // Bloquear eliminación si hay mobiliarios asociados

            // Convertir el Enum Estado a string en la base de datos
            modelBuilder.Entity<Mobiliario>()
                .Property(m => m.Estado)
                .HasConversion<string>(); // Almacenar como texto

            // Relación muchos a muchos entre Reservas y Apartamentos
            modelBuilder.Entity<Reservas>()
                .HasMany(r => r.Apartamentos)
                .WithMany(a => a.Reserva)
                .UsingEntity(j =>
                {
                    j.ToTable("ReservasApartamentos");
                    // No se agregan datos iniciales (seed data)
                });

            // Configuración específica para Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email) // Asegurar que el correo sea único
                .IsUnique();
        }
    }
}