using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sklepix.Data.Entities
{

    public class AisleRowEntity
    {
        [Key]
        public int Id { get; set; }
        public int RowNumber;

    }

}
