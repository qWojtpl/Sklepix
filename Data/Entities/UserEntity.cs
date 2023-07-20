using Microsoft.AspNetCore.Identity;
namespace Sklepix.Data.Entities
{
    public class UserEntity : IdentityUser
    {
        public string? Description { get; set; }
        public int Type { get; set; } = 0;
    }

}
