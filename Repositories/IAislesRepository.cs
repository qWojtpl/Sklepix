using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public interface IAislesRepository
    {
        List<AisleEntity> List();
        AisleEntity? One(int id);
        void Add(AisleEntity model);
        void Edit(int id, AisleEntity model);
        bool Delete(int id);
        bool Delete(AisleEntity model);
        void Save();
    }
}
