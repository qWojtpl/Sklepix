using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public interface IUsersRepository
    {
        List<UserEntity> List();
        UserEntity? One(string id);
        UserEntity? OneByName(string? name);
        Task<bool> Add(UserEntity model, string password, string? roles);
        Task<bool> Edit(string id, UserEntity model, string password, string? roles);
        Task<bool> Delete(string id);
        Task<bool> Delete(UserEntity model);
        void Save();
        bool CanManage(string id);
        Task<bool> AssignRoles(UserEntity user, string roles);
    }
}
