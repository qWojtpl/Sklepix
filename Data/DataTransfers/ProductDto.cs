using Sklepix.Data.Entities;

namespace Sklepix.Data.DataTransfers
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public CategoryEntity? Category { get; set; }
        public AisleEntity Aisle { get; set; }
        public int Row { get; set; }

    }
}
