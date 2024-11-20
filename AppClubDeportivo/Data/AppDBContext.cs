using Microsoft.EntityFrameworkCore;
using AppClubDeportivo.Models;

namespace AppClubDeportivo.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ejercicio> Ejercicios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Actividad> Actividades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(tb => {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(col => col.Nombre).HasMaxLength(30);
                tb.Property(col => col.Apellido).HasMaxLength(30);
                tb.Property(col => col.Correo).HasMaxLength(50);
                tb.Property(col => col.Telefono).HasMaxLength(20);
                tb.Property(col => col.DNI).HasMaxLength(20);
                tb.Property(col => col.Contraseña).HasMaxLength(256);
                tb.Property(col => col.Rol).HasMaxLength(20);
            });

            modelBuilder.Entity<Ejercicio>(tb =>
            {
                tb.HasKey(e => e.Id);
                tb.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
                tb.Property(e => e.Descripcion).HasMaxLength(500);
                tb.Property(e => e.ImagenUrl).HasMaxLength(255);
                tb.Property(e => e.VideoUrl).HasMaxLength(255);
            });

            modelBuilder.Entity<Actividad>(tb =>
            {
                tb.HasKey(a => a.Id);
                tb.Property(a => a.Nombre).HasMaxLength(100).IsRequired();
                tb.Property(a => a.Descripcion).HasMaxLength(500);
                tb.Property(a => a.Horario).HasMaxLength(100);
            });
        }

        // Método para agregar un usuario con contraseña encriptada
        public void AddUserWithEncryptedPassword(Usuario usuario, string password)
        {
            usuario.SetPassword(password);
            Usuarios.Add(usuario);
            SaveChanges();
        }

        // Método para inicializar administradores si no existen
        public void InicializarAdministrador()
        {
            // Comprobamos si los administradores ya existen en la base de datos
            if (!Usuarios.Any(u => u.DNI == "42199974"))
            {
                var admin1 = new Usuario
                {
                    Nombre = "Admin",
                    Apellido = "Principal",
                    DNI = "42199974",
                    Correo = "admin@appclubdeportivo.com",
                    Telefono = "1131377110",
                    Rol = "Administrador"
                };

                admin1.SetPassword("admin123");
                Usuarios.Add(admin1);
            }

            // Agregamos el segundo administrador
            if (!Usuarios.Any(u => u.DNI == "38324957"))
            {
                var admin2 = new Usuario
                {
                    Nombre = "Agus",
                    Apellido = "Agis",
                    DNI = "38324957",
                    Correo = "agus.agis@appclubdeportivo.com",
                    Telefono = "1131377111",
                    Rol = "Administrador"
                };

                admin2.SetPassword("agus123");
                Usuarios.Add(admin2);
            }

            // Agregamos el tercer administrador
            if (!Usuarios.Any(u => u.DNI == "40011019"))
            {
                var admin3 = new Usuario
                {
                    Nombre = "Iván",
                    Apellido = "Barrios",
                    DNI = "40011019",
                    Correo = "ivan.barrios@appclubdeportivo.com",
                    Telefono = "1131377112",
                    Rol = "Administrador"
                };

                admin3.SetPassword("ivan123");
                Usuarios.Add(admin3);
            }

            // Guardamos los cambios en la base de datos
            SaveChanges();
        }
    }
}
