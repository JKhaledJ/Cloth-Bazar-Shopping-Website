using ClothBazar.Services;
using ClothBazar.web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothBazar.web.Controllers
{
    public class WidgetsController : Controller
    {
        // GET: Widgets
        ProductsWidgetViewModel productsWidgetViewModel = new ProductsWidgetViewModel();
        

        public ActionResult Products(bool isLatestProducts, int? CategoryID=0)
        {
            productsWidgetViewModel.IsLatestProduct = isLatestProducts;
            if (isLatestProducts)
            {
                productsWidgetViewModel.Products = ProductsService.ClassObj.GetLatestProducts(4);
            }
            else if(CategoryID.HasValue && CategoryID.Value > 0)
            {
                productsWidgetViewModel.Products = ProductsService.ClassObj.GetProductsByCategory(CategoryID.Value,4);
                productsWidgetViewModel.CategoryID = CategoryID.Value;
            }
            else
            {
                productsWidgetViewModel.Products = ProductsService.ClassObj.GetProducts(1, 8);
            }
            return PartialView(productsWidgetViewModel);
        }
    }
}