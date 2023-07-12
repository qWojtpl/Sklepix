using Sklepix.Data;
using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public class AisleRowsRepository : IAisleRowsRepository
    {
        private readonly AppDbContext _context;

        public AisleRowsRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public List<AisleRowEntity> List()
        {
            return _context.AisleRows
                .Select(n => n)
                .ToList();
        }

        public AisleRowEntity? One(int id)
        {
            return _context.AisleRows
                .FirstOrDefault(n => n.Id == id);
        }

        public void Add(AisleRowEntity model)
        {
            _context.AisleRows.Add(model);
        }

        public void Edit(int id, AisleRowEntity model)
        {
            model.Id = id;
            Delete(id);
            Add(model);
        }

        public bool Delete(int id)
        {
            AisleRowEntity? entity = _context.AisleRows.FirstOrDefault(n => n.Id == id);
            if(entity == null)
            {
                return false;
            }
            _context.AisleRows.Remove(entity);
            return true;
        }

        public bool Delete(AisleRowEntity model)
        {
            _context.AisleRows.Remove(model);
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }

}
