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
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Evento> Eventos { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder) //nos permite definir las caracteristicas de nuestra tabla
        {
            //base.OnModelCreating(modelBuilder);

             
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
        }

        // étodo para agregar un usuario con contraseña encriptada
        public void AddUserWithEncryptedPassword(Usuario usuario, string password)
        {
            usuario.SetPassword(password);
            Usuarios.Add(usuario);
            SaveChanges();
        }

        public void InicializarAdministrador()
        {

            if (!Usuarios.Any(u => u.DNI == "42199974"))
            {
                var admin = new Usuario
                {
                    Nombre = "Admin",
                    Apellido = "Principal",
                    DNI = "42199974",
                    Correo = "admin@appclubdeportivo.com",
                    Telefono = "1131377110",
                    Rol = "Administrador"
                };


                admin.SetPassword("admin123");


                Usuarios.Add(admin);
                SaveChanges();
            }
        }


    }
    
}

        
        

