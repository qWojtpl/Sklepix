using Sklepix.Data.Entities;

namespace Sklepix.Models.ViewModels
{
    public class UserVm
    {

        public string Id { get; set; }
        public string Mail { get; set; }
        public string? Description { get; set; }
        public int Type { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

    }
}
