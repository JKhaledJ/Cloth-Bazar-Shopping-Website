using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClothBazar.Entities;
using ClothBazar.web.Models;
using ClothBazar.Services;
using ClothBazar.web.ViewModels;

namespace ClothBazar.web.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            var registration = new Registration();
            registration.FirstName = model.Name;
            registration.Email = model.Email;
            registration.Country = model.Country;
            registration.Password = model.Password;
            registration.ConfirmPassword = model.ConfirmPassword;
            if (ModelState.IsValid)
            {
                AccountService.Instance.Register(registration);
                ModelState.Clear();
                ViewBag.Message = registration.FirstName  + " successfully registered.";
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Registration user)
        {
            using (var context = new myContext())
            {
                var usr = AccountService.Instance.Login(user);
                if (usr != null)
                {
                    Session["userID"] = usr.UserId.ToString();
                    Session["userName"] = usr.FirstName.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is required.");
                }
            }
            return View();
        }
        public ActionResult LoggedIn()
        {
            if (Session["userID"] != null)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}