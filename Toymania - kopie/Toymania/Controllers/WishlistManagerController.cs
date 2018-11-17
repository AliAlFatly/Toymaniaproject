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
    public class WishlistManagerController : Controller
    {

        // GET: WishlistManager
        TSE15 db = new TSE15();

        public ActionResult Index()
        {
            var w = WishlistManager.GW(this.HttpContext);
            var viewModel = new WishlistViewModel
            {
                WT = w.GWT()
            };
            return View(viewModel);
        }

        public ActionResult IndexR(int RecordToRemoveId)
        {
            RFW(RecordToRemoveId);
            var w = WishlistManager.GW(this.HttpContext);
            var viewModel = new WishlistViewModel
            {
                WT = w.GWT()
            };
            return View(viewModel);
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
            var w = WishlistManager.GW(this.HttpContext);
            string toyName = db.Wishlist.Single(t => t.WishlistId == id).Toy.ToysName;
            w.RFW(id);
        }

    }
}