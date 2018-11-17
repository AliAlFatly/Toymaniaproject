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
        TSE15 db = new TSE15();
        //private readonly UserManager<ApplicationUser> _userManager;

        //public HomeController(UserManager<ApplicationUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        public ActionResult Index()
        {

            var cart = ShoppingCart.GC(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CT = cart.GCT(),
                CartTotal = cart.GT(),
            };

            return View(viewModel);
        }

        public ActionResult IndexR(int RecordToRemoveId)
        {

            RFC(RecordToRemoveId);

            var cart = ShoppingCart.GC(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CT = cart.GCT(),
                CartTotal = cart.GT(),
            };

            return View(viewModel);
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

        [HttpPost]
        public void RFC(int id)
        {
            var cart = ShoppingCart.GC(this.HttpContext);
            string toyName = db.Cart.Single(t => t.RecordId == id).Toy.ToysName;
            cart.RFC(id);
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GC(this.HttpContext);

            ViewData["CartCount"] = cart.GCount();
            return PartialView("CartSummary");
        }


    }

}