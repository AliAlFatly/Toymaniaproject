using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toymania.Models;
using Toymania.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Toymania.Controllers;
using Toymania.Services;

namespace Toymania.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        ApplicationDbContext db = new ApplicationDbContext();
        //private readonly UserManager<ApplicationUser> _userManager;

        //public HomeController(UserManager<ApplicationUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        public ActionResult Index()
        {

            var cart = ShoppingCartService.GetCart(this.HttpContext);
            var UserEmail = HttpContext.User.Identity.Name;
            var Model = new ShoppingCartViewModel
            {
                CartToy = cart.GetCartToys(),
                CartTotal = cart.GetTotalPrice(),
                U = db.Users.Select(e => e.Email == UserEmail) == null
            };

            return View(Model);
        }

        public ActionResult IndexE()
        {

            var cart = ShoppingCartService.GetCart(this.HttpContext);
            var UserEmail = HttpContext.User.Identity.Name;
            var Model = new ShoppingCartViewModel
            {
                CartToy = cart.GetCartToys(),
                CartTotal = cart.GetTotalPrice(),
                U = db.Users.Select(e => e.Email == UserEmail) == null
            };

            return View(Model);
        }

        public ActionResult IndexR(int RecordToRemoveId)
        {
            try
            {
                RemoveFromCart(RecordToRemoveId);

                var cart = ShoppingCartService.GetCart(this.HttpContext);
                var UserEmail = HttpContext.User.Identity.Name;
                var Model = new ShoppingCartViewModel
                {
                    CartToy = cart.GetCartToys(),
                    CartTotal = cart.GetTotalPrice(),
                    U = db.Users.Select(e => e.Email == UserEmail) == null
                };

                return View(Model);
            }
            catch
            {
                return RedirectToAction("Index");
            }


        }

        public ActionResult AddToCart(int id)
        {
            var addedItem = db.Toy.Single(t => t.ToysId == id);
            var cart = ShoppingCartService.GetCart(this.HttpContext);

            cart.AddToCart(addedItem);

            return RedirectToAction("index");
        }

        //[HttpPost]
        public JsonResult RemoveFromCart(int id)
        {
            var cart = ShoppingCartService.GetCart(this.HttpContext);
            string toyName = db.Cart.Single(t => t.RecordId == id).Toy.ToysName;
            var c = db.Cart.SingleOrDefault(i => i.RecordId == id);
            bool result = false;
            if(c != null)
            {
                cart.RemoveFromCart(id);
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
            //return PartialView("PartialCart");
            
            
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCartService.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }

        public void AddPartial(int id)
        {
            var addedItem = db.Toy.Single(t => t.ToysId == id);
            var cart = ShoppingCartService.GetCart(this.HttpContext);

            cart.AddToCart(addedItem);
            //return PartialView("CartSummary");
        }


    }

}