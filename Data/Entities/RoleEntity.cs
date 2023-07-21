using Microsoft.AspNetCore.Identity;

namespace Sklepix.Data.Entities
{
    public class RoleEntity : IdentityRole
    {

        public string? Description { get; set; }

    }
}
