using System.ComponentModel.DataAnnotations;

namespace Sklepix.Models.ViewModels
{
    public class AisleVm
    {
        public int Id { get; set; }
        [Display(Name = "Aisle name")]
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
