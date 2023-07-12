using Sklepix.Data;
using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public interface IProductsRepository
    {
        List<ProductEntity> List();
        ProductEntity? One(int id);
        void Add(ProductEntity model);
        void Edit(int id, ProductEntity model);
        bool Delete(int id);
        bool Delete(ProductEntity model);
        void Save();
        AppDbContext GetRaw();
    }
}
