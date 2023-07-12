using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public interface ICategoriesRepository
    {
        List<CategoryEntity> List();
        CategoryEntity? One(int id);
        void Add(CategoryEntity model);
        void Edit(int id, CategoryEntity model);
        bool Delete(int id);
        bool Delete(CategoryEntity model);
        void Save();
    }
}
