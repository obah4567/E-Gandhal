using E_Gandhal.src.Domain.Models.Authentification;
using E_Gandhal.src.Domain.Models.Students;
using E_Gandhal.src.Domain.Models.Teachers;
using Microsoft.EntityFrameworkCore;

namespace E_Gandhal.src.Infrastructure.ApplicationDBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Register> Users => Set<Register>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Matiere> Matieres => Set<Matiere>();
        public DbSet<Note> Notes => Set<Note>();
        public DbSet<Classe> Classes => Set<Classe>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Student -> Classe

            modelBuilder.Entity<Student>()
                .HasOne<Classe>()
                .WithMany()
                .HasForeignKey(s => s.ClasseId)
                .OnDelete(DeleteBehavior.NoAction); // Empêche la suppression d'une classe si des étudiants y sont liés

            // Note -> Student
            modelBuilder.Entity<Note>()
                .HasOne(n => n.Students)
                .WithMany()
                .HasForeignKey(n => n.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Note -> Matiere
            modelBuilder.Entity<Note>()
                .HasOne(n => n.Matieres)
                .WithMany()
                .HasForeignKey(n => n.MatiereId)
                .OnDelete(DeleteBehavior.Restrict);

            // Matiere -> Classe
            modelBuilder.Entity<Matiere>()
                .HasOne(m => m.Classe)
                .WithMany(c => c.Matieres)
                .HasForeignKey(m => m.ClasseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Matiere -> Teacher
            modelBuilder.Entity<Matiere>()
                .HasOne(m => m.Teacher)
                .WithMany(t => t.Matieres)
                .HasForeignKey(m => m.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
