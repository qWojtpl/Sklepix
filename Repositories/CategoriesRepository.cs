using Sklepix.Data;
using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly AppDbContext _context;

        public CategoriesRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public List<CategoryEntity> List()
        {
            return _context.Categories
                .Select(n => n)
                .ToList();
        }

        public CategoryEntity? One(int id)
        {
            return _context.Categories
                .FirstOrDefault(n => n.Id == id);
        }

        public void Add(CategoryEntity model)
        {
            _context.Categories.Add(model);
        }

        public void Edit(int id, CategoryEntity model)
        {
            model.Id = id;
            Delete(id);
            Add(model);
        }

        public bool Delete(int id)
        {
            CategoryEntity? entity = _context.Categories.FirstOrDefault(n => n.Id == id);
            if(entity == null)
            {
                return false;
            }
            _context.Categories.Remove(entity);
            return true;
        }

        public bool Delete(CategoryEntity model)
        {
            _context.Categories.Remove(model);
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }

}
