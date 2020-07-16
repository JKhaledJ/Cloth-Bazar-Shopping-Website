using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClothBazar.Entities;
using ClothBazar.Services;
using ClothBazar.web.ViewModels;

namespace ClothBazar.web.Controllers
{
    public class ProductController : Controller
    {
      //  ProductsService productsService = new ProductsService(); 
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductTable(string search, int? pageNo)
        {
            ProductSearchViewModel productSearchViewModel = new ProductSearchViewModel();
            pageNo = pageNo.HasValue ?pageNo.Value>0 ? pageNo.Value :1 : 1;
            var pageSize = ConfigService.Instance.PageSize();
           // productSearchViewModel.Products = ProductsService.ClassObj.GetProducts(productSearchViewModel.pageNo);
            var totalRecords = ProductsService.ClassObj.GetProductsCount(search);
            productSearchViewModel.Products = ProductsService.ClassObj.GetProducts(search, pageNo.Value, pageSize);
            productSearchViewModel.Pager = new Pager(totalRecords, pageNo, pageSize);
            return PartialView(productSearchViewModel);//to return only the creation table without header, footer.
        }

        #region Creation
        [HttpGet]
        public ActionResult Create()
        {
            var categories = CategoriesService.Instance.GetCategories();
            return PartialView(categories); //to return only the creation table without header, footer.
        }
        [HttpPost]
        public ActionResult Create(CategoryViewModel model)
        {
            var newProduct = new Product();
            var category = new Category();
            category = CategoriesService.Instance.GetCategory(model.CategoryID);
            newProduct.Name = model.Name;
            newProduct.Description = model.Description;
            newProduct.Price = model.Price;
            newProduct.ImageURL = model.ImageURL;
            newProduct.category = category;

            ProductsService.ClassObj.SaveProduct(newProduct);
            return RedirectToAction("ProductTable");
            
        }
        #endregion

        #region Updating
        [HttpGet]
        public ActionResult Edit(int id)
        {
            NewProductViewModel model = new NewProductViewModel();
            ProductViewModel productViewModel = new ProductViewModel();

            productViewModel.Product= ProductsService.ClassObj.GetProduct(id);
            model.ID = productViewModel.Product.ID;
            model.Name = productViewModel.Product.Name;
            model.Description = productViewModel.Product.Description;
            model.Price = Convert.ToInt32(productViewModel.Product.Price);
            model.ImageURL = productViewModel.Product.ImageURL;
            model.AvailableCategories = CategoriesService.Instance.GetCategories();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(NewProductViewModel model)
        {
            var existingProduct = ProductsService.ClassObj.GetProduct(model.ID);
            existingProduct.Name = model.Name;
            existingProduct.Description = model.Description;
            existingProduct.Price = model.Price;
            existingProduct.category = null;
            existingProduct.CategoryID = model.CategoryID;
            
            if (!string.IsNullOrEmpty(model.ImageURL))
            {
                existingProduct.ImageURL = model.ImageURL;
            }
            ProductsService.ClassObj.UpdateProduct(existingProduct);
            return RedirectToAction("ProductTable");
        }
        #endregion

        #region Deleting
        [HttpPost]
        public ActionResult Delete(Product product)
        {
            ProductsService.ClassObj.DeleteProduct(product);
            return RedirectToAction("ProductTable");
        }
        #endregion

        [HttpGet]
        public ActionResult Details(int id)
        {
            ProductViewModel model = new ProductViewModel();

            model.Product = ProductsService.ClassObj.GetProduct(id);
            return View(model);
        }
    }
}