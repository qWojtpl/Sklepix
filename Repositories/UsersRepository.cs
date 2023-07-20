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

        public UsersRepository(AppDbContext appDbContext, UserManager<UserEntity> userManager)
        {
            this._context = appDbContext;
            this._userManager = userManager;
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

        public async Task<bool> Add(UserEntity model, string password)
        {
            await _userManager.CreateAsync(model, password);
            return true;
        }

        public async Task<bool> Edit(string id, UserEntity model, string? password)
        {
            if(password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(model);
                await _userManager.ResetPasswordAsync(model, token, password);
            }
            model.Id = id;
            Console.WriteLine(await _userManager.UpdateAsync(model));
            return true;
        }

        public async Task<bool> Delete(string id)
        {
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
            await _userManager.DeleteAsync(model);
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }

}
