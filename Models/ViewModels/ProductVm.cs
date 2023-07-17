using Sklepix.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sklepix.Models.ViewModels
{
    public class ProductVm
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Count")]
        public float Count { get; set; }
        [Display(Name = "Price")]
        public float Price { get; set; }
        public float Margin { get; set; }
        public float PriceWithMargin { get; set; }
        public float PotentialIncome { get; set; }
        public string? Category { get; set; }
        public string? Aisle { get; set; }
        public int Row { get; set; }

    }
}
