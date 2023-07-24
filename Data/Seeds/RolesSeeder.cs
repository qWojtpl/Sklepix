using Microsoft.AspNetCore.Identity;
using Sklepix.Data.Entities;

namespace Sklepix.Data.Seeds
{
    public class RolesSeeder
    {

        public static async Task<bool> Seed(RoleManager<RoleEntity> roleManager)
        {
            string[] names = new string[] { 
                "CategoryView", "CategoryAdd", "CategoryEdit", "CategoryRemove", 
                "AisleView", "AisleAdd", "AisleEdit", "AisleRemove", "AisleRowManage",
                "ProductView", "ProductAdd", "ProductEdit", "ProductRemove",
                "UserView", "UserAdd", "UserEdit", "UserRemove"
            };
            foreach(string name in names) 
            {
                if((await roleManager.FindByNameAsync(name)) != null)
                {
                    continue;
                }
                var result = await roleManager.CreateAsync(new RoleEntity()
                {
                    Name = name
                });
                Console.WriteLine(result);
            }
            return true;
        }

    }
}
