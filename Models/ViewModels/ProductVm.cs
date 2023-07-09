using Sklepix.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sklepix.Models.ViewModels
{
    public class ProductVm
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Count")]
        public int Count { get; set; }
        [Display(Name = "Price")]
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public int AisleId { get; set; }
        public int Row { get; set; }

    }
}
