using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public interface IUsersRepository
    {
        List<UserEntity> List();
        UserEntity? One(string id);
        Task<bool> Add(UserEntity model, string password);
        Task<bool> Edit(string id, UserEntity model, string password);
        Task<bool> Delete(string id);
        Task<bool> Delete(UserEntity model);
        void Save();
        bool CanManage(string id);
    }
}
