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
        // AFILIACION <-> EPS
        modelBuilder.Entity<Afiliacion>()
            .HasOne(a => a.EPS)
            .WithMany(e => e.Afiliaciones)
            .HasForeignKey(a => a.EPSID);

        // ROL <-> USUARIO
        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Rol)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(u => u.RolID);

        // USUARIO <-> PACIENTE
        modelBuilder.Entity<Paciente>()
            .HasOne(p => p.Usuario)
            .WithMany()
            .HasForeignKey(p => p.UsuarioFK);

        // USUARIO <-> ENFERMERO
        modelBuilder.Entity<Enfermero>()
            .HasOne(e => e.Usuario)
            .WithMany()
            .HasForeignKey(e => e.UsuarioFK);

        // USUARIO <-> NOTIFICACION
        modelBuilder.Entity<Notificacion>()
            .HasOne(n => n.Usuario)
            .WithMany(u => u.Notificaciones)
            .HasForeignKey(n => n.UsuarioID);

        // USUARIO <-> LOGSISTEMA
        modelBuilder.Entity<LogSistema>()
            .HasOne(l => l.Usuario)
            .WithMany(u => u.Logs)
            .HasForeignKey(l => l.UsuarioID);

        // PACIENTE <-> CITA
        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Paciente)
            .WithMany(p => p.Citas)
            .HasForeignKey(c => c.PacienteID);

        // EXAMEN <-> CITA
        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Examen)
            .WithMany(e => e.Citas)
            .HasForeignKey(c => c.ExamenID);

        // ESTADOCITA <-> CITA
        modelBuilder.Entity<Cita>()
            .HasOne(c => c.EstadoCita)
            .WithMany(e => e.Citas)
            .HasForeignKey(c => c.EstadoID);

        // CITA <-> REPROGRAMACION
        modelBuilder.Entity<Reprogramacion>()
            .HasOne(r => r.Cita)
            .WithMany(c => c.Reprogramaciones)
            .HasForeignKey(r => r.CitaID);

        // CITA <-> HISTORIALCITA
        modelBuilder.Entity<HistorialCita>()
            .HasOne(h => h.Cita)
            .WithMany(c => c.HistorialCitas)
            .HasForeignKey(h => h.CitaID);

        // CITA <-> RESULTADO (1:1)
        modelBuilder.Entity<Resultado>()
            .HasOne(r => r.Cita)
            .WithOne(c => c.Resultado)
            .HasForeignKey<Resultado>(r => r.CitaID);

        // HISTORIALCITA <-> ESTADOCITA (Doble FK)
        modelBuilder.Entity<HistorialCita>()
            .HasOne(h => h.EstadoAnteriorNavigation)
            .WithMany(e => e.HistorialCitasAnterior)
            .HasForeignKey(h => h.EstadoAnterior)
            .OnDelete(DeleteBehavior.Restrict); 
            
        modelBuilder.Entity<HistorialCita>()
            .HasOne(h => h.EstadoNuevoNavigation)
            .WithMany(e => e.HistorialCitasNuevo)
            .HasForeignKey(h => h.EstadoNuevo)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);

        }
    }
}