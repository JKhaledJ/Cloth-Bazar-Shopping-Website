using ClothBazar.Entities;
using ClothBazar.Services;
using ClothBazar.web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace ClothBazar.web.Controllers
{
    public class CategoryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CategoryTable(string search, int? pageNo)
        {
            CategorySearchViewModel categorySearchViewModel = new CategorySearchViewModel();
            categorySearchViewModel.SearchTerm = search;
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

            var totalRecords = CategoriesService.Instance.GetCategoriesCount(search);
            var ListingPageSize = int.Parse(ConfigService.Instance.GetConfig("ListingPageSize").Value);
            categorySearchViewModel.Categories = CategoriesService.Instance.GetCategories(search, pageNo.Value);

            if (categorySearchViewModel.Categories != null)
            {
                categorySearchViewModel.Pager = new Pager(totalRecords, pageNo, ListingPageSize);

                return PartialView(categorySearchViewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult Create()
        {

            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(NewCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newCategory = new Category();
                newCategory.Name = model.Name;
                newCategory.Description = model.Description;
                newCategory.ImageURL = model.ImageURL;
                newCategory.isFeatured = model.isFeatured;

                CategoriesService.Instance.SaveCategory(newCategory);
                return RedirectToAction("CategoryTable");
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = CategoriesService.Instance.GetCategory(id);
            return PartialView(category);
        }
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            CategoriesService.Instance.UpdateCategory(category);
            return RedirectToAction("CategoryTable");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var category = CategoriesService.Instance.GetCategory(id);
            return PartialView(category);
        }
        [HttpPost]
        public ActionResult Delete(Category category)
        {
            CategoriesService.Instance.DeleteCategory(category);
            return RedirectToAction("CategoryTable");
        }
    }
}