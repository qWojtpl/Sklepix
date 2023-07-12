namespace Sklepix.Models.ViewModels
{
    public class ProductCreateVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public float Count { get; set; }
        public float Price { get; set; }
        public string? CategoryName { get; set; }
        public string? AisleName { get; set; }
        public int Row { get; set; }
        public List<string>? CategoriesNames { get; set; }
        public List<string>? AisleNames { get; set; }
        public Dictionary<string, List<int>>? AisleRows { get; set; }
    }
}
