using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ClothBazar.Entities;
using System.Data.Entity.Infrastructure;

namespace ClothBazar.Database
{
    public class CBContext:DbContext,IDisposable
    {
        public CBContext() : base("ClothBazarConnection")
        {

        }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Config> Configurations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
