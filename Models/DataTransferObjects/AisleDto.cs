namespace Sklepix.Models.DataTransferObjects
{
    public class AisleDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Rows { get; set; } = "";
        public string? UserName { get; set; }
        public List<string> UserNames { get; set; } = new List<string>();

    }

}
