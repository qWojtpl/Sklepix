using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sklepix.Data.Entities
{
    //[Index(nameof(RowNumber), nameof(Aisle), IsUnique = true)]
    public class AisleRowEntity
    {
        [Key]
        public int Id { get; set; }
        public int RowNumber;
        //public AisleEntity Aisle { get; set; }

    }
}
