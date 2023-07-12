using Microsoft.EntityFrameworkCore;
using Sklepix.Data.Entities;

namespace Sklepix.Data.Seeds
{
    public class CategoriesSeeder
    {

        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<CategoryEntity>().HasData(new List<CategoryEntity>()
            {
                new CategoryEntity() { 
                    Id = 1,
                    Name = "Baking",
                    Description = "Breads, loafs"
                },
                new CategoryEntity()
                {
                    Id = 2,
                    Name = "Drinks",
                    Description = "Drinks such as water, juices"
                },
                new CategoryEntity()
                {
                    Id = 3,
                    Name = "Frozen food",
                    Description = "Frozen pizzas, fries, vegetables"
                }
            });
        }

    }
}
