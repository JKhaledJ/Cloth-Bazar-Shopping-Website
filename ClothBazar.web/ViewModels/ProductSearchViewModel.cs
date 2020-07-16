using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClothBazar.Entities;

namespace ClothBazar.web.ViewModels
{
    public class ProductSearchViewModel
    {
        public string SearchTerm { get; set; }
        public List<Product> Products { get; set; }
        public int pageNo { get; set; }
        public Pager Pager { get; set; }
    }
}