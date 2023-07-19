using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public interface IUsersRepository
    {
        List<UserEntity> List();
        UserEntity? One(int id);
        void Add(UserEntity model);
        void Edit(int id, UserEntity model);
        bool Delete(int id);
        bool Delete(UserEntity model);
        void Save();
    }
}
