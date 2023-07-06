using System.ComponentModel.DataAnnotations;

namespace Sklepix.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
    }

}
