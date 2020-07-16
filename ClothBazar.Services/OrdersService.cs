using ClothBazar.Database;
using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Services
{
    public class OrdersService
    {
        #region Singleton
        public static OrdersService Instance
        {
            get
            {
                if (instance == null) instance = new OrdersService();

                return instance;
            }
        }
        private static OrdersService instance { get; set; }
        private OrdersService()
        {
        }
        #endregion

        public List<Order> SearchOrders(string userID,string status, int? pageNo, int pageSize)
        {
            using (var context = new CBContext())
            {
                var orders = context.Orders.ToList();
                
                if (!string.IsNullOrEmpty(userID))
                {
                    orders = orders.Where(x => x.UserID.ToString().ToLower().Contains(userID.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(status))
                {
                    orders = orders.Where(x => x.Status.ToLower().Contains(status.ToLower())).ToList();
                }
                
                return orders.Skip((pageNo.Value - 1) * pageSize).Take(pageSize).ToList();
            }
        }
        
        public int SearchOrdersCount(string userID, string status)
        {
            using (var context = new CBContext())
            {
                var orders = context.Orders.ToList();

                if (!string.IsNullOrEmpty(userID))
                {
                    orders = orders.Where(x => x.UserID.ToString().ToLower().Contains(userID.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(status))
                {
                    orders = orders.Where(x => x.Status.ToLower().Contains(status.ToLower())).ToList();
                }

                return orders.Count;
            }
        }

        public Order GetOrderByID(int id)
        {
            using (var context = new CBContext())
            {
                return context.Orders.Where(x => x.ID == id)
                    .Include(x=>x.OrderItems)
                    .Include("OrderItems.Product")
                    .FirstOrDefault();
            }
        }

        public bool UpdateOrderStatus(int id, string status)
        {
            using (var context = new CBContext())
            {
                var order = context.Orders.Find(id);
                //if (order != null)
                //{
                //    order.Status = status;
                //    context.SaveChanges();
                //    return true;
                //}
                //return false;

                order.Status = status;
                return context.SaveChanges() > 0; //return num of effected rows.
                                                  //returns true if effected and false otherwise.

            }
        }
    }
}
