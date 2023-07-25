using Sklepix.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sklepix.Data.Entities
{
    public class TaskEntity
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public UserEntity User { get; set; }
        public int Status { get; set; }
        public DateTime AssignTime { get; set; }
        public DateTime Deadline { get; set; }
        public int Priority { get; set; }
        public string? Comment { get; set; }

    }
}
