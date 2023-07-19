using Sklepix.Data;
using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _context;

        public UsersRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public List<UserEntity> List()
        {
            return _context.Users
                .Select(n => n)
                .ToList();
        }

        public UserEntity? One(int id)
        {
            return _context.Users
                .FirstOrDefault(n => n.Id == id);
        }

        public void Add(UserEntity model)
        {
            _context.Users.Add(model);
        }

        public void Edit(int id, UserEntity model)
        {
            model.Id = id;
            Delete(id);
            Add(model);
        }

        public bool Delete(int id)
        {
            UserEntity? entity = One(id);
            if(entity == null)
            {
                return false;
            }
            _context.Users.Remove(entity);
            return true;
        }

        public bool Delete(UserEntity model)
        {
            _context.Users.Remove(model);
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }

}
