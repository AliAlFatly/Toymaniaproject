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

            var cart = ShoppingCart.GC(this.HttpContext);
            var a = true;
            var u = HttpContext.User.Identity.Name;
            var A = db.Users.Select(e => e.Email == u) == null;
            if (!A)
            {
                a = false;
            }
            
            var viewModel = new ShoppingCartViewModel
            {
                CT = cart.GCT(),
                CartTotal = cart.GT(),
                U = a
            };

            return View(viewModel);
        }

        public ActionResult IndexE()
        {

            var cart = ShoppingCart.GC(this.HttpContext);
            var a = true;
            var u = HttpContext.User.Identity.Name;
            var A = db.Users.Select(e => e.Email == u) == null;
            if (!A)
            {
                a = false;
            }

            var viewModel = new ShoppingCartViewModel
            {
                CT = cart.GCT(),
                CartTotal = cart.GT(),
                U = a
            };

            return View(viewModel);
        }

        public ActionResult IndexR(int RecordToRemoveId)
        {
            try
            {
                RFC(RecordToRemoveId);

                var cart = ShoppingCart.GC(this.HttpContext);
                var a = true;
                var u = HttpContext.User.Identity.Name;
                var A = db.Users.Select(e => e.Email == u) == null;
                if (!A)
                {
                    a = false;
                }

                var viewModel = new ShoppingCartViewModel
                {
                    CT = cart.GCT(),
                    CartTotal = cart.GT(),
                    U = a
                };

                return View(viewModel);
            }
            catch
            {
                return RedirectToAction("Index");
            }


        }

        public ActionResult ATC(int id)
        {
            var addedItem = db.Toy.Single(t => t.ToysId == id);
            var cart = ShoppingCart.GC(this.HttpContext);

            cart.ATC(addedItem, id);

            return RedirectToAction("index");
        }
        //a
        //[HttpPost]
        //public ActionResult RFC(int id)
        //{
        //    var cart = ShoppingCart.GC(this.HttpContext);

        //    string toyName = db.Cart.Single(t => t.RecordId == id).Toy.ToysName;

        //    int toyCount = cart.RFC(id);

        //    var results = new ShoppingCartRemoveViewModel
        //    {
        //        Message = Server.HtmlEncode(toyName) + " has been removed from your shopping cart.",
        //        CartTotal = cart.GT(),
        //        CartCount = cart.GCount(),
        //        ToyCount = toyCount,
        //        DeleteId = id

        //    };

        //    ActionResult a = Index();
        //    return Json(results);
        //}

        //[HttpPost]
        public JsonResult RFC(int id)
        {
            var cart = ShoppingCart.GC(this.HttpContext);
            string toyName = db.Cart.Single(t => t.RecordId == id).Toy.ToysName;
            var c = db.Cart.SingleOrDefault(i => i.RecordId == id);
            bool result = false;
            if(c != null)
            {
                cart.RFC(id);
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
            //return PartialView("PartialCart");
            
            
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GC(this.HttpContext);

            ViewData["CartCount"] = cart.GCount();
            return PartialView("CartSummary");
        }

        public void AddPartial(int id)
        {
            var addedItem = db.Toy.Single(t => t.ToysId == id);
            var cart = ShoppingCart.GC(this.HttpContext);

            cart.ATC(addedItem, id);
            //return PartialView("CartSummary");
        }


    }

}