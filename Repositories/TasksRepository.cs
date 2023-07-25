using Sklepix.Data;
using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        private readonly AppDbContext _context;

        public TasksRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public List<TaskEntity> List()
        {
            return _context.Tasks
                .Select(n => n)
                .ToList();
        }

        public TaskEntity? One(int id)
        {
            return _context.Tasks
                .FirstOrDefault(n => n.Id == id);
        }

        public void Add(TaskEntity model)
        {
            _context.Tasks.Add(model);
        }

        public void Edit(int id, TaskEntity model)
        {
            model.Id = id;
            Delete(id);
            Add(model);
        }

        public bool Delete(int id)
        {
            TaskEntity? entity = One(id);
            if(entity == null)
            {
                return false;
            }
            _context.Tasks.Remove(entity);
            return true;
        }

        public bool Delete(TaskEntity model)
        {
            _context.Tasks.Remove(model);
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }

}
