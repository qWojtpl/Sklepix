using Sklepix.Data.Entities;

namespace Sklepix.Models.ViewModels
{
    public class TaskVm
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string UserName { get; set; }
        public int Status { get; set; }
        public string StatusString { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime Deadline { get; set; }
        public int Priority { get; set; }
        public string? Comment { get; set; }
        public string? DeadlineColor { get; set; }

    }
}
