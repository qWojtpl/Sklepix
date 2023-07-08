using Sklepix.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sklepix.Models.ViewModels
{
    public class AisleVm
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Aisle name")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Rows")]
        public string Rows { get; set; } = "";

    }
}
