using Microsoft.AspNetCore.Identity;
using Sklepix.Data.Entities;

namespace Sklepix.Data.Seeds
{
    public class UsersSeeder
    {

        public static async Task<bool> Seed(UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager)
        {
            if((await userManager.FindByEmailAsync("admin@sklepix.test")) != null)
            {
                return false;
            }
            UserEntity model = new UserEntity()
            {
                UserName = "admin@sklepix.test",
                Email = "admin@sklepix.test",
                Description = "Sklepix default admin",
                Type = 1
            };
            await userManager.CreateAsync(model, "ADMIN$123z");
            foreach(RoleEntity role in roleManager.Roles.ToList())
            {
                await userManager.AddToRoleAsync(model, role.Name);
            }
            return true;
        }

    }
}
