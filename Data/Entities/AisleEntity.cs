using System.ComponentModel.DataAnnotations;

namespace Sklepix.Data.Entities
{
    public class AisleEntity
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Aisle name")]
        public string name { get; set; }
    }
}
