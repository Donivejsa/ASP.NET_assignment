using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjektHantering.Models;

namespace ProjektHantering.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurera Budget-egenskapens precision
            modelBuilder.Entity<Project>()
                .Property(p => p.Budget)
                .HasPrecision(18, 2); // 18 siffror totalt, varav 2 decimaler
        }
    }
}