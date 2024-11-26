using E_Gandhal.src.Domain.Models.Authentification;
using E_Gandhal.src.Domain.Models.Student;
using Microsoft.EntityFrameworkCore;

namespace E_Gandhal.src.Infrastructure.ApplicationDBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        
        }

        public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
        public DbSet<Student> Students => Set<Student>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
