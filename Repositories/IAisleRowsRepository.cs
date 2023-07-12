using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public interface IAisleRowsRepository
    {
        List<AisleRowEntity> List();
        AisleRowEntity? One(int id);
        void Add(AisleRowEntity model);
        void Edit(int id, AisleRowEntity model);
        bool Delete(int id);
        bool Delete(AisleRowEntity model);
        void Save();
    }
}
