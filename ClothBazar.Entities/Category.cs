﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Entities
{
    public class Category:BaseEntity
    {
        public string ImageURL { get; set; }
        public List<Product> products { get; set; }
        public bool isFeatured { get; set; }
    }
}
