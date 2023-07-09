using System.ComponentModel.DataAnnotations;

namespace Sklepix.Data.Entities
{
    public class AisleRowEntity
    {
        [Key]
        public int Id { get; set; }
        public int RowNumber { get; set; }
        public AisleEntity Aisle { get; set; }
    }
}
