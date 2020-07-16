using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClothBazar.Entities;
using ClothBazar.Services;
using ClothBazar.web.Code;
using ClothBazar.web.ViewModels;
namespace ClothBazar.web.Controllers
{
    public class ShopController : Controller
    {
        CheckOutViewModel model = new CheckOutViewModel();
        ShopViewModel shopViewModel = new ShopViewModel();
        FilterProductsViewModel filterProductsViewModel = new FilterProductsViewModel();
        public ActionResult Index(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy, int? pageNo)
        {
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            shopViewModel.SearchTerm = searchTerm;
            var pageSize = ConfigService.Instance.PageSize();
            shopViewModel.FeaturedCategories = CategoriesService.Instance.FeaturedGetCategories();
            shopViewModel.MaximumPrice = ProductsService.ClassObj.GetMaximumPrice();
            shopViewModel.Products = ProductsService.ClassObj.SearchProducts(searchTerm, minimumPrice, maximumPrice, categoryID, sortBy, pageNo.Value, pageSize);
            shopViewModel.sortBy = sortBy;
            //filterProductsViewModel.sortBy = sortBy;
            shopViewModel.PageNo = pageNo;
            shopViewModel.CategoryID = categoryID;
            //filterProductsViewModel.CategoryID = categoryID;
            int totalCount = ProductsService.ClassObj.SearchProductsCount(searchTerm, minimumPrice, maximumPrice, categoryID, sortBy);
            shopViewModel.Pager = new Pager(totalCount, pageNo, pageSize);
            return View(shopViewModel);

        }
        public ActionResult FilterProducts(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy, int? pageNo)
        {
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            filterProductsViewModel.SearchTerm = searchTerm;
            filterProductsViewModel.sortBy = sortBy;
            shopViewModel.sortBy = sortBy;
            filterProductsViewModel.CategoryID = categoryID;
            var pageSize = ConfigService.Instance.ShopPageSize();
            filterProductsViewModel.Products = ProductsService.ClassObj.SearchProducts(searchTerm, minimumPrice, maximumPrice, categoryID, sortBy, pageNo.Value, pageSize);
            int totalCount = ProductsService.ClassObj.SearchProductsCount(searchTerm, minimumPrice, maximumPrice, categoryID, sortBy);
            filterProductsViewModel.Pager = new Pager(totalCount, pageNo, pageSize);
            return PartialView(filterProductsViewModel);
        }
        public ActionResult Checkout()
        {
            var CookieProducts = Request.Cookies["productCookie"];
            int userID = Convert.ToInt32(Session["userID"]);
            if (userID != 0)
            {
                if (CookieProducts != null && !string.IsNullOrEmpty(CookieProducts.Value))
                {
                    model.CartProductIDs = CookieProducts.Value.Split('-').Select(x => int.Parse(x)).ToList();
                    model.CartProducts = ProductsService.ClassObj.GetProducts(model.CartProductIDs);
                    model.User = AccountService.Instance.GetUserByID(userID);

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
            return View(model);
        }
        public JsonResult PlaceOrder(string ProductsIDs)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (!string.IsNullOrEmpty(ProductsIDs))
            {
                //Distinct() checks the items repeated multiple times and does not send multiple times to server.
                var Ids = ProductsIDs.Split(new char[] { '-' }).Select(x => int.Parse(x)).Distinct().ToList();
                var BoughtProducts = ProductsService.ClassObj.GetProducts(Ids);
                var productQuantity = ProductsIDs.Split('-').Select(x => int.Parse(x)).ToList();
                Order newOrder = new Order();

                newOrder.UserID = Convert.ToInt32(Session["userID"]);
                newOrder.OrderedAt = DateTime.Now;
                newOrder.Status = "Pending";
                newOrder.TotalAmount = BoughtProducts.Sum(x => x.Price * productQuantity.Where(y => y == x.ID).Count());

                newOrder.OrderItems = new List<OrderItem>();
                newOrder.OrderItems.AddRange(BoughtProducts.Select(x => new OrderItem() { ProductID = x.ID, Quantity = productQuantity.Where(y => y == x.ID).Count() }));

                var rowsEffected = ShopService.Instance.SaveOrder(newOrder);

                result.Data = new { Success = true , Row = rowsEffected };
            }
            else
            {
                result.Data = new { Success= false };
            }
            return result;
        }
    }
}