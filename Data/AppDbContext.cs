using Microsoft.EntityFrameworkCore;
using Sklepix.Data.Entities;
using Sklepix.Data.Seeds;

namespace Sklepix.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<AisleEntity> Aisles { get; set; }
        public DbSet<AisleRowEntity> AisleRows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=Sklepix;Trusted_Connection=True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CategoriesSeeder.Seed(modelBuilder);
            AislesSeeder.Seed(modelBuilder);
        }

    }
}
