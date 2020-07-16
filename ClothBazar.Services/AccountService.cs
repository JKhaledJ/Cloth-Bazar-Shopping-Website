using ClothBazar.Database;
using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Services
{
    public class AccountService
    {
        public static AccountService Instance
        {
            get
            {
                if (instance == null) instance = new AccountService();

                return instance;
            }
        }
        private static AccountService instance { get; set; }
        AccountService()
        {
        }
        public void Register(Registration registration)
        {
                using (var context = new CBContext())
                {
                    context.Registrations.Add(registration);
                    context.SaveChanges();
                }
            
        }
        public Registration Login(Registration user)
        {
            using (var context = new CBContext())
            {
                return context.Registrations.Single(x => x.Email == user.Email && x.Password == user.Password);
            }
        }
        public Registration GetUserByID(int id)
        {
            using (var context = new CBContext())
            {
                return context.Registrations.Find(id);
            }
        }
    }
}
