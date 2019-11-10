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
    public class WishlistManagerController : Controller
    {

        // GET: WishlistManager
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var Wishlist = WishlistService.GetWishlist(this.HttpContext);
            var Model = new WishlistViewModel
            {
                Wishlist = Wishlist.GetToys()
            };
            return View(Model);
        }

        public ActionResult IndexR(int r)
        {
            try
            {
                var Wishlist = WishlistService.GetWishlist(this.HttpContext);
                RemoveFromWishlist(r);

                var Model = new WishlistViewModel
                {
                    Wishlist = Wishlist.GetToys()
                };
                return View(Model);
            }
            catch
            {
                return RedirectToAction("Index");
            }


        }

        public ActionResult IndexRA(int i, int r)
        {
            try
            {
                var addedItem = db.Toy.Single(t => t.ToysId == i);
                var cart = ShoppingCartService.GetCart(this.HttpContext);

                cart.AddToCart(addedItem);
                RemoveFromWishlist(r);
                var Wishlist = WishlistService.GetWishlist(this.HttpContext);
                var Model = new WishlistViewModel
                {
                    Wishlist = Wishlist.GetToys()
                };
                return View(Model);
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }

        public ActionResult AddToWishlist(int id)
        {
            var addedItem = db.Toy.Single(t => t.ToysId == id);
            var Wishlist = WishlistService.GetWishlist(this.HttpContext);
            Wishlist.AddToWishlist(addedItem, id);
            
            return RedirectToAction("index");
        }

        [HttpPost]
        public void RemoveFromWishlist(int id)
        {
            if(id != null)
            {
                var w = WishlistService.GetWishlist(this.HttpContext);
                string toyName = db.Wishlist.Single(t => t.WishlistId == id).Toy.ToysName;
                w.DeleteFromWishlist(id);
            }

        }

        public void AddPartial(int id)
        {
            var addedItem = db.Toy.Single(t => t.ToysId == id);
            var Wishlist = WishlistService.GetWishlist(this.HttpContext);
            Wishlist.AddToWishlist(addedItem, id);
        }

    }
}