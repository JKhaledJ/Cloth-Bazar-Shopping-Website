using ClothBazar.Entities;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClothBazar.web.Models
{
    public class myContext:DbContext
    {
        public myContext() : base("ClothBazarConnection")
        {

        }
        public DbSet<Registration> Registrations { get; set; }
    }
}