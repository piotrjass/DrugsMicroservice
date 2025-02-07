using DrugsMicroservice.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace DrugsMicroservice.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Substance> Substances { get; set; }
        
        public DbSet<Disease> Diseases { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Drug>().ToTable("Drugs");
            modelBuilder.Entity<Substance>().ToTable("Substances");
            modelBuilder.Entity<Disease>().ToTable("Diseases");
        }
    }
}