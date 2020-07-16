using System;
using ClothBazar.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothBazar.web.ViewModels
{
    public class HomeViewModels
    {
        public List<Category> FeaturedProducts { get; set; }
        public List<Category> FeaturedCategories { get; set; }
    }
}