﻿using System;
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
    public class WishlistManagerController : Controller
    {

        // GET: WishlistManager
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var w = WishlistManager.GW(this.HttpContext);
            var viewModel = new WishlistViewModel
            {
                WT = w.GWT()
            };
            return View(viewModel);
        }

        public ActionResult IndexR(int r)
        {
            try
            {
                var w = WishlistManager.GW(this.HttpContext);
                //var identity = w.RWID();
                //var toyexist = !w.ToyDontExist(db.Toy.Find(r), identity);
                //if (toyexist)
                //{
                //    RFW(r);
                //}
                RFW(r);

                var viewModel = new WishlistViewModel
                {
                    WT = w.GWT()
                };
                return View(viewModel);
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
                var cart = ShoppingCart.GC(this.HttpContext);
                //var w = WishlistManager.GW(this.HttpContext);
                //var identity = w.RWID();
                //var toyexist = !w.ToyDontExist(db.Toy.Find(r), identity);
                //if (toyexist)
                //{
                //    cart.ATC(addedItem, i);
                //    RFW(r);
                //}

                cart.ATC(addedItem, i);
                RFW(r);
                var w = WishlistManager.GW(this.HttpContext);
                var viewModel = new WishlistViewModel
                {
                    WT = w.GWT()
                };
                return View(viewModel);
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }

        public ActionResult ATW(int id)
        {
            var addedItem = db.Toy.Single(t => t.ToysId == id);
            var w = WishlistManager.GW(this.HttpContext);
            w.ATW(addedItem, id);
            
            return RedirectToAction("index");
        }

        [HttpPost]
        public void RFW(int id)
        {
            if(id != null)
            {
                var w = WishlistManager.GW(this.HttpContext);
                string toyName = db.Wishlist.Single(t => t.WishlistId == id).Toy.ToysName;
                w.RFW(id);
            }

        }

        public void AddPartial(int id)
        {
            var addedItem = db.Toy.Single(t => t.ToysId == id);
            var w = WishlistManager.GW(this.HttpContext);
            w.ATW(addedItem, id);
            //return PartialView("CartSummary");
        }

    }
}