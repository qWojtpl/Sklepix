namespace Sklepix.Models.DataTransferObjects
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string? Description { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }

}
