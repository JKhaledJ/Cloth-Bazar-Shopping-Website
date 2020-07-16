using ClothBazar.Services;
using ClothBazar.web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothBazar.web.Controllers
{
    public class OrderController : Controller
    {
        OrderViewModels orderViewModels = new OrderViewModels();
        OrderDetailsViewModels orderDetailsViewModels = new OrderDetailsViewModels();
        public ActionResult Index(string userID, string status, int? pageNo)
        {
            orderViewModels.UserID = userID;
            orderViewModels.Status = status;

            int pageSize = ConfigService.Instance.PageSize();
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            orderViewModels.Orders = OrdersService.Instance.SearchOrders(userID, status, pageNo, pageSize);
           
            int totalRecords = OrdersService.Instance.SearchOrdersCount(userID,status);
            orderViewModels.Pager = new Pager(totalRecords, pageNo, pageSize);

            return View(orderViewModels);
        }
        public ActionResult Details(int ID)
        {
            orderDetailsViewModels.Orders= OrdersService.Instance.GetOrderByID(ID);
            if (orderDetailsViewModels.Orders != null)
            {
                orderDetailsViewModels.OrderBy = AccountService.Instance.GetUserByID(orderDetailsViewModels.Orders.UserID);
            }
            orderDetailsViewModels.AvailableStatuses = new List<string>() { "Pending", "In progress", "Delivered" };
            return View(orderDetailsViewModels);
        }
        public JsonResult ChangeStatus(int orderID, string status)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            bool isSuccess = OrdersService.Instance.UpdateOrderStatus(orderID, status);
            result.Data = new { Success = isSuccess };
            return result;
        }
    }
}