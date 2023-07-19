using Microsoft.AspNetCore.Identity;

namespace Sklepix.Data.Entities
{
    public class UserEntity : IdentityUser
    {
        public int Id { get; set; }
        public string? Description { get; set; }
    }

}
