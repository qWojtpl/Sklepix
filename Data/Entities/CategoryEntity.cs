using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Sklepix.Data.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class CategoryEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

    }

}
