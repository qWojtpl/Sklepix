using Microsoft.EntityFrameworkCore;
using Sklepix.Data.Entities;

namespace Sklepix.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=Sklepix;Trusted_Connection=True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }

    }
}
