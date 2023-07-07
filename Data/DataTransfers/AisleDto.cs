using Sklepix.Data.Entities;

namespace Sklepix.Data.DataTransfers
{
    public class AisleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<AisleRowEntity>? Rows { get; set; }
    }

}
