using Microsoft.EntityFrameworkCore;
using Proyecto.Model;

namespace Proyecto.Data
{
    public class ProyectoDbContext : DbContext
    {
        public ProyectoDbContext(DbContextOptions<ProyectoDbContext> options) : base(options) {}

        public DbSet<Afiliacion> Afiliaciones { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<EPS> EPSs { get; set; }
        public DbSet<Enfermero> Enfermeros { get; set; }
        public DbSet<EstadoCita> Estados { get; set; }
        public DbSet<Examen> Examenes { get; set; }
        public DbSet<HistorialCita> HistorialCitas { get; set; }
        public DbSet<LogSistema> LogsSistema { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Reprogramacion> Reprogramaciones { get; set; }
        public DbSet<Resultado> Resultados { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ejemplo de restricciones únicas y relaciones
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Documento)
                .IsUnique();

            modelBuilder.Entity<Paciente>()
                .HasIndex(p => p.Correo)
                .IsUnique();

            modelBuilder.Entity<Afiliacion>()
                .HasIndex(a => new { a.TipoDocumento, a.Documento})
                .IsUnique();

            // Puedes agregar más configuraciones aquí si tus modelos tienen relaciones especiales
        }
    }
}