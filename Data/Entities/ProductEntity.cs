﻿using System.ComponentModel.DataAnnotations;

namespace Sklepix.Data.Entities
{
    public class ProductEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public CategoryEntity? Category { get; set; }
        public AisleEntity? Aisle { get; set; }
        public AisleRowEntity? Row { get; set; }
    }

}
