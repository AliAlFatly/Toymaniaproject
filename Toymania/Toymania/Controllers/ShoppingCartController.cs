using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toymania.Models;
using Toymania.ViewModels;


namespace Toymania.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        TSE7 db = new TSE7();
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


        public ActionResult ATC(int id)
        {
            var addedItem = db.Toy.Single(t => t.ToysId == id);
            var cart = ShoppingCart.GC(this.HttpContext);

            cart.ATC(addedItem);

            return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult RFC(int? id)
        {
            var cart = ShoppingCart.GC(this.HttpContext);

            string toyName = db.Cart.Single(t => t.RecordId == id).Toy.ToysName;

            int? toyCount = cart.RFC(id);

            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(toyName) + " has been removed from your shopping cart.",
                CartTotal = cart.GT(),
                CartCount = cart.GCount(),
                ToyCount = toyCount,
                DeleteId = id

            };
            return Json(results);
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