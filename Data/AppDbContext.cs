using Microsoft.EntityFrameworkCore;
using proyecto.Models;

namespace proyecto.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Afiliacion> Afiliaciones { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Enfermero> Enfermeros { get; set; }
        public DbSet<EPS> EPSs { get; set; }
        public DbSet<EstadoCita> EstadoCitas { get; set; }
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
            // Paciente -> Usuario (uno a uno)
            modelBuilder.Entity<Paciente>()
                .HasOne(p => p.Usuario)
                .WithOne(u => u.Paciente)
                .HasForeignKey<Paciente>(p => p.UsuarioFK);

            // Usuario -> Rol (muchos a uno)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolFK);

            // Cita -> Usuario (muchos a uno)
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Citas)
                .HasForeignKey(c => c.UsuarioFK);

            // Cita -> EstadoCita (muchos a uno)
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Estado)
                .WithMany(e => e.Citas)
                .HasForeignKey(c => c.EstadoFK);

            // Cita -> Enfermero (muchos a uno, opcional)
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Enfermero)
                .WithMany(e => e.Citas)
                .HasForeignKey(c => c.EnfermeroFK);

            // Cita -> Examen (muchos a uno)
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Examen)
                .WithMany(e => e.Citas)
                .HasForeignKey(c => c.ExamenFK);

            // Resultado -> Cita (uno a uno)
            modelBuilder.Entity<Resultado>()
                .HasOne(r => r.Cita)
                .WithOne(c => c.Resultado)
                .HasForeignKey<Resultado>(r => r.CitaFK);

            // HistorialCita -> Cita (muchos a uno)
            modelBuilder.Entity<HistorialCita>()
                .HasOne(h => h.Cita)
                .WithMany(c => c.Historial)
                .HasForeignKey(h => h.CitaFK);

            // HistorialCita -> Usuario (muchos a uno)
            modelBuilder.Entity<HistorialCita>()
                .HasOne(h => h.Usuario)
                .WithMany(u => u.HistorialCitas)
                .HasForeignKey(h => h.UsuarioAccion);

            // Reprogramacion -> Cita (muchos a uno)
            modelBuilder.Entity<Reprogramacion>()
                .HasOne(r => r.Cita)
                .WithMany(c => c.Reprogramaciones)
                .HasForeignKey(r => r.CitaFK);

            // Afiliacion -> Usuario (uno a uno)
            modelBuilder.Entity<Afiliacion>()
                .HasOne(a => a.Usuario)
                .WithOne(u => u.Afiliacion)
                .HasForeignKey<Afiliacion>(a => a.UsuarioFK);

            // Afiliacion -> EPS (muchos a uno)
            modelBuilder.Entity<Afiliacion>()
                .HasOne(a => a.EPS)
                .WithMany(e => e.Afiliaciones)
                .HasForeignKey(a => a.EPSFK);

            // Notificacion -> Usuario (muchos a uno)
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.Usuario)
                .WithMany(u => u.Notificaciones)
                .HasForeignKey(n => n.UsuarioFK);
            
            // Enfermero -> Usuario (uno a uno)
            modelBuilder.Entity<Enfermero>()
                .HasOne(e => e.Usuario)
                .WithOne(u => u.Enfermero)
                .HasForeignKey<Enfermero>(e => e.UsuarioFK);

            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                    {
                        property.SetMaxLength(255);
                    }
                }
            }
        }
    }
}
