using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public interface ITasksRepository
    {
        List<TaskEntity> List();
        TaskEntity? One(int id);
        void Add(TaskEntity model);
        void Edit(int id, TaskEntity model);
        bool Delete(int id);
        bool Delete(TaskEntity model);
        void Save();
    }
}
