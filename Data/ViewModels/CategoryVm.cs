using System.ComponentModel.DataAnnotations;

namespace Sklepix.Data.ViewModels
{
    public class CategoryVm
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Category name")]
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
