namespace Sklepix.Models.DataTransferObjects
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public float Count { get; set; }
        public float Price { get; set; }
        public string? CategoryName { get; set; }
        public string? AisleName { get; set; }
        public int Row { get; set; }

    }
}
