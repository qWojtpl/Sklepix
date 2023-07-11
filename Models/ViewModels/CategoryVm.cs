using System.ComponentModel.DataAnnotations;

namespace Sklepix.Models.ViewModels
{
    public class CategoryVm
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
