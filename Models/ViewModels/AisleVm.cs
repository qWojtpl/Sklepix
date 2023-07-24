using Sklepix.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sklepix.Models.ViewModels
{
    public class AisleVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? UserName { get; set; }
        public bool IsUsed { get; set; }
        public List<AisleRowEntity> Rows { get; set; }

    }
}
