using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothBazar.web.ViewModels
{
    public class OrderViewModels
    {
        public string UserID { get; set; }
        public List<Order> Orders { get; set; }
        public string Status { get; set; }
        public Pager Pager { get; set; }
    }
    public class OrderDetailsViewModels
    {
        public Order Orders { get; set; }
        public Registration OrderBy { get; set; }
        public List<string> AvailableStatuses { get; set; }
    }

}