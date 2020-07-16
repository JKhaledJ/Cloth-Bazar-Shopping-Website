using ClothBazar.Database;
using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Services
{
    public class ConfigService
    {
        public static ConfigService Instance
        {
            get
            {
                if (instance == null) instance = new ConfigService();

                return instance;
            }
        }
        private static ConfigService instance { get; set; }
        private ConfigService()
        {
        }

        public Config GetConfig(string Key)
        {
            using(var context= new CBContext())
            {
                return context.Configurations.Where(x => x.Key == Key).FirstOrDefault();
            }

        }

        public int PageSize()
        {
            using (var context = new CBContext())
            {
                var pageSizeConfig= context.Configurations.Find("ListingPageSize");
                return pageSizeConfig != null ? int.Parse(pageSizeConfig.Value): 10;
            }
        }
        public int ShopPageSize()
        {
            using (var context = new CBContext())
            {
                var pageSizeConfig = context.Configurations.Find("ShopPageSize");
                return pageSizeConfig != null ? int.Parse(pageSizeConfig.Value) : 10;
            }
        }
    }
}
