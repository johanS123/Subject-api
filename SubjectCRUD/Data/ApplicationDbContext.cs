using Microsoft.EntityFrameworkCore;
using SubjectCRUD.Models;

namespace SubjectCRUD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Registration> Registrations { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Estudiante ↔ Inscripciones
            modelBuilder.Entity<Registration>()
                .HasOne(i => i.Student)
                .WithMany(e => e.Registrations)
                .HasForeignKey(i => i.StudentId);

            // Relación Materia ↔ Inscripciones
            modelBuilder.Entity<Registration>()
                .HasOne(i => i.Subject)
                .WithMany(m => m.Registrations)
                .HasForeignKey(i => i.SubjectId);

            // Relación Profesor ↔ Materias
            modelBuilder.Entity<Subject>()
                .HasOne(m => m.Teacher)
                .WithMany(p => p.Subjects)
                .HasForeignKey(m => m.TeacherId);
        }

    }
}
