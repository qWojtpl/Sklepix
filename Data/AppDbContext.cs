using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sklepix.Data.Entities;
using Sklepix.Data.Seeds;

namespace Sklepix.Data
{
    public class AppDbContext : IdentityDbContext<UserEntity>
    {

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<AisleEntity> Aisles { get; set; }
        public DbSet<AisleRowEntity> AisleRows { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CategoriesSeeder.Seed(modelBuilder);
            AislesSeeder.Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
