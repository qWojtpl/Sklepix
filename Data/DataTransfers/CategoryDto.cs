using System.ComponentModel.DataAnnotations;

namespace Sklepix.Data.DataTransfers
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
