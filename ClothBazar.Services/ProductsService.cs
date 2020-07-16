using ClothBazar.Entities;
using ClothBazar.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ClothBazar.Services
{
    public class ProductsService
    {
        #region Singleton
        public static ProductsService ClassObj
        {
            get
            {
                if (privateObj == null) privateObj = new ProductsService();

                return privateObj;
            }
        }
        private static ProductsService privateObj { get; set; }
        private ProductsService()
        {
        }
        #endregion

        //pageNo is for pagenation
        public List<Product> GetProducts(int pageNo)
        {
            int pageSize = int.Parse(ConfigService.Instance.GetConfig("ListingPageSize").Value);
            //var context = new CBContext();
            //return context.Products.ToList();
            using (var context = new CBContext())
            {
                //Skip() is to skip no of products of the previous pages and take() is to take
                //new amount of products in every page.
               return context.Products.OrderBy(x => x.ID).Skip((pageNo - 1) * pageSize).Take(pageSize).Include(x => x.category).ToList();
               // return context.Products.Include(x=>x.category).ToList();
            }
        }

        public List<Product> GetProducts(string search, int pageNo,int pageSize)
        {

            using (var context = new CBContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Products.Where(product => product.Name != null &&
                            product.Name.ToLower().Contains(search.ToLower()))
                            .OrderBy(x => x.ID)
                            .Skip((pageNo - 1) * pageSize)
                            .Take(pageSize)
                            .Include(x => x.category)
                            .ToList();
                }
                else
                {
                    return context.Products
                             .OrderBy(x => x.ID)
                             .Skip((pageNo - 1) * pageSize)
                             .Take(pageSize)
                             .Include(x => x.category)
                             .ToList();
                }
            }
        }
        public int GetProductsCount(string search)
        {

            using (var context = new CBContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Products.Where(product => product.Name != null &&
                            product.Name.ToLower().Contains(search.ToLower()))
                            .Count();
                }
                else
                {
                    return context.Products.Count();
                }
            }
        }
        public List<Product> GetAllProducts()
        {
            using (var context = new CBContext())
            {

                return context.Products.Include(x => x.category).ToList();
            }
        }

        //This is for latest products according to their category.
        public List<Product> GetProducts(int pageNo,int pageSize)
        {
            using (var context = new CBContext())
            {
                return context.Products.OrderByDescending(x => x.ID).Skip((pageNo - 1) * pageSize).Take(pageSize).Include(x => x.category).ToList();
            }
        }

        public List<Product> GetProductsByCategory(int CategoryID, int pageSize)
        {
            using (var context = new CBContext())
            {
                return context.Products.Where(x=>x.category.ID==CategoryID).OrderByDescending(x => x.ID).Take(pageSize).Include(x => x.category).ToList();
            }
        }
        public List<Product> GetLatestProducts(int numberOfProducts)
        {
            using (var context = new CBContext())
            {
                return context.Products.OrderByDescending(x => x.ID).Take(numberOfProducts).Include(x => x.category).ToList();
            }
        }
        public void SaveProduct(Product product)
        {
            using(var context =new CBContext())
            {
                context.Entry(product.category).State = System.Data.Entity.EntityState.Unchanged;
                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        public Product GetProduct(int id)
        {
            using (var context = new CBContext())
            {
              //  return context.Products.Find(id);
                return context.Products.Where(x=>x.ID==id).Include(x=>x.category).SingleOrDefault();

            }
        }

        public List<Product> GetProducts(List<int> id)
        {
            using (var context = new CBContext())
            {
                return context.Products.Where(product => id.Contains(product.ID)).ToList() ;
            }
        }
        public void UpdateProduct(Product product)
        {
            using (var context = new CBContext())
            {
                context.Entry(product).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteProduct(Product product)
        {
            using (var context = new CBContext())
            {
                context.Entry(product).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public int GetMaximumPrice()
        {
            using (var context = new CBContext())
            {
                return Convert.ToInt32(context.Products.Max(x => x.Price));
            }
        }

        public List<Product> SearchProducts(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryID,int? sortBy,int? pageNo, int pageSize)
        {
            using (var context = new CBContext())
            {
                var products=context.Products.ToList();
                if (categoryID.HasValue)
                {
                    products = products.Where(x => x.category.ID == categoryID).ToList();
                }
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    products=products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
                }
                if (minimumPrice.HasValue)
                {
                    products = products.Where(x => x.Price>=minimumPrice.Value).ToList();
                }
                if (maximumPrice.HasValue)
                {
                    products = products.Where(x => x.Price <= maximumPrice.Value).ToList();
                }
                if (sortBy.HasValue)
                {
                    switch (sortBy.Value)
                    {
                        case 2:
                            products = products.OrderByDescending(x => x.ID).ToList();
                            break;
                        case 3:
                            products = products.OrderBy(x => x.Price).ToList();
                            break;
                        case 4:
                            products = products.OrderByDescending(x => x.Price).ToList();
                            break;
                        default:
                            products = products.OrderBy(x => x.ID).ToList();
                            break;
                    }
                }
                return products.Skip((pageNo.Value - 1) * pageSize).Take(pageSize).ToList();
            }
        }
        public int SearchProductsCount(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy)
        {
            using (var context = new CBContext())
            {
                var products = context.Products.ToList();
                if (categoryID.HasValue)
                {
                    products = products.Where(x => x.category.ID == categoryID).ToList();
                }
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
                }
                if (minimumPrice.HasValue)
                {
                    products = products.Where(x => x.Price >= minimumPrice.Value).ToList();
                }
                if (maximumPrice.HasValue)
                {
                    products = products.Where(x => x.Price <= maximumPrice.Value).ToList();
                }
                if (sortBy.HasValue)
                {
                    switch (sortBy.Value)
                    {
                        case 2:
                            products = products.OrderByDescending(x => x.ID).ToList();
                            break;
                        case 3:
                            products = products.OrderBy(x => x.Price).ToList();
                            break;
                        case 4:
                            products = products.OrderByDescending(x => x.Price).ToList();
                            break;
                        default:
                            products = products.OrderBy(x => x.ID).ToList();
                            break;
                    }
                }
                return products.Count;
            }
        }

    }
}
