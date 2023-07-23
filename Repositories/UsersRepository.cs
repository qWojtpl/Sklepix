using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sklepix.Data;
using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;

        public UsersRepository(AppDbContext appDbContext, UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager)
        {
            this._context = appDbContext;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public List<UserEntity> List()
        {
            return _context.Users
                .ToList();
        }

        public UserEntity? One(string id)
        {
            return _context.Users
                .FirstOrDefault(n => n.Id == id);
        }

        public async Task<bool> Add(UserEntity model, string password, string? roles)
        {
            await _userManager.CreateAsync(model, password);
            if(roles != null)
            {
                await AssignRoles(model, roles);
            }
            return true;
        }

        public async Task<bool> Edit(string id, UserEntity model, string? password, string? roles)
        {
            if(!CanManage(id))
            {
                return false;
            }
            if(password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(model);
                await _userManager.ResetPasswordAsync(model, token, password);
            }
            model.Id = id;
            await _userManager.UpdateAsync(model);
            if(roles != null)
            {
                await _userManager.RemoveFromRolesAsync(model, await _userManager.GetRolesAsync(model));
                await AssignRoles(model, roles);
            }
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            if (!CanManage(id))
            {
                return false;
            }
            UserEntity? entity = One(id);
            if(entity == null)
            {
                return false;
            }
            await _userManager.DeleteAsync(entity);
            return true;
        }

        public async Task<bool> Delete(UserEntity model)
        {
            if(!CanManage(model.Id))
            {
                return false;
            }
            await _userManager.DeleteAsync(model);
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool CanManage(string id)
        {
            UserEntity? entity = One(id);
            if (entity == null)
            {
                return false;
            }
            if (entity.Type == 1)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AssignRoles(UserEntity user, string roles)
        {
            string[] split = roles.Split(";");
            for (int i = 0; i < split.Length - 1; i++)
            {
                if ((await _roleManager.FindByNameAsync(split[i])) != null)
                {
                    await _userManager.AddToRoleAsync(user, split[i]);
                }
            }
            return true;
        }

    }

}
