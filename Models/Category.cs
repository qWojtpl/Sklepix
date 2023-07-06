﻿using System.ComponentModel.DataAnnotations;

namespace Sklepix.Models
{
    public class Category
    {
        [Key]
        [Display(Name= "ID")]
        public int ID { get; set; }
        [Display(Name = "Category name")]
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

}
