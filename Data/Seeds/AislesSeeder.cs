using Microsoft.EntityFrameworkCore;
using Sklepix.Data.Entities;

namespace Sklepix.Data.Seeds
{
    public class AislesSeeder
    {
        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<AisleEntity>().HasData(new List<AisleEntity>()
            {
                new AisleEntity() {
                    Id = 1,
                    Name = "Fridges",
                    Description = "Fridge aisle"
                },
                new AisleEntity()
                {
                    Id = 2,
                    Name = "Left aisle",
                    Description = "Aisle at the left from the enterance"
                },
                new AisleEntity()
                {
                    Id = 3,
                    Name = "Right aisle",
                    Description = "Aisle at the right from the enterance"
                }
            });
        }
    }
}
