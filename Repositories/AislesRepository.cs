using Sklepix.Data;
using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public class AislesRepository : IAislesRepository
    {
        private readonly AppDbContext _context;

        public AislesRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public List<AisleEntity> List()
        {
            return _context.Aisles
                .Select(n => n)
                .ToList();
        }

        public AisleEntity? One(int id)
        {
            return _context.Aisles
                .FirstOrDefault(n => n.Id == id);
        }

        public void Add(AisleEntity model)
        {
            _context.Aisles.Add(model);
        }

        public void Edit(int id, AisleEntity model)
        {
            model.Id = id;
            Delete(id);
            Add(model);
        }

        public bool Delete(int id)
        {
            AisleEntity? entity = One(id);
            if(entity == null)
            {
                return false;
            }
            _context.Aisles.Remove(entity);
            return true;
        }

        public bool Delete(AisleEntity model)
        {
            _context.Aisles.Remove(model);
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }

}
