using E_Gandhal.src.Domain.Models.Authentification;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
