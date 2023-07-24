using System.ComponentModel.DataAnnotations;

namespace Sklepix.Data.Entities
{
    public class AisleEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public UserEntity? User { get; set; }
    }
}