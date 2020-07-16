using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Entities
{
    public class Product:BaseEntity
    {
        public string ImageURL { get; set; }
        public int CategoryID { get; set; }
        public virtual Category category { get; set; }
        [Range(1,10000)]
        public decimal Price { get; set; }
    }
}
