using ClothBazar.Entities;
using ClothBazar.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace ClothBazar.Services
{
    public class CategoriesService
    {
        #region CategorySingleton
        public static CategoriesService Instance
        {
            get
            {
                if (instance == null) instance = new CategoriesService();

             return instance;
            }
        }
        private static CategoriesService instance { get; set; }
        CategoriesService()
        {
        }


        #endregion

        

        public int GetCategoriesCount(string search)
        {
            using (var context = new CBContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Categories.Where(category => category.Name != null &&
                            category.Name.ToLower().Contains(search.ToLower())).Count();
                }
                else
                {
                    return context.Categories.Count();
                }
            }
        }
        public List<Category> GetCategories(string search,int pageNo)
        {
            
            int pageSize =int.Parse(ConfigService.Instance.GetConfig("ListingPageSize").Value);
            using (var context = new CBContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Categories.Where(category => category.Name != null &&
                            category.Name.ToLower().Contains(search.ToLower()))
                            .OrderBy(x => x.ID)
                            .Skip((pageNo - 1) * pageSize)
                            .Take(pageSize)
                            .Include(x => x.products)
                            .ToList();
                }
                else
                {
                    return context.Categories
                             .OrderBy(x => x.ID)
                             .Skip((pageNo - 1) * pageSize)
                             .Take(pageSize)
                             .Include(x => x.products)
                             .ToList();
                }
            }
        }
        public List<Category> GetCategories()
        {
            using (var context = new CBContext())
            {
                return context.Categories.ToList();
            }
        }
        public List<Category> FeaturedGetCategories()
        {
            using (var context = new CBContext())
            {
                return context.Categories.Where(x=>x.isFeatured && x.ImageURL !=null).ToList();
            }
        }
        public void SaveCategory(Category category)
        {
            using(var context =new CBContext())
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }

        public Category GetCategory(int id)
        {
            using (var context = new CBContext())
            {
                return context.Categories.Find(id);
            }
        }
        public void UpdateCategory(Category category)
        {
            using (var context = new CBContext())
            {
                context.Entry(category).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteCategory(Category Category)
        {
            using (var context = new CBContext())
            {
                var category = context.Categories.Where(x => x.ID == Category.ID).Include(x => x.products).FirstOrDefault();
                context.Products.RemoveRange(category.products);
                //context.Categories.Remove(category); this is same as below.
                context.Entry(category).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
