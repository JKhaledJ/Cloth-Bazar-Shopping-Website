using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ClothBazar.web.ViewModels
{
    public class CategoryViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string ImageURL { get; set; }
        //public List<Product> Products { get; set; }

        public int CategoryID { get; set; }

    }
    public class CategorySearchViewModel
    {
        public string SearchTerm { get; set; }
        public List<Category> Categories { get; set; }
        public Pager Pager { get; set; }
    }

    public class NewCategoryViewModel
    {
        [Required]
        [MinLength(5),MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public bool isFeatured { get; set; }
    }
}