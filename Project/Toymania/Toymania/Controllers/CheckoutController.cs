using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toymania.Models;
using Toymania.Services;
using Toymania.ViewModel;
using Toymania.ViewModels;


namespace Toymania.Controllers
{
    
    public class CheckoutController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        //redirect to logged/unlogged checkout forum
        public ActionResult AddressAndPaymentManager()
        {
            var Context = this.HttpContext;
            var cart = ShoppingCartService.GetCart(this.HttpContext);
            if (cart.GetTotalPrice() < 0.01m)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }

            if (db.Users.SingleOrDefault(n => n.Email == Context.User.Identity.Name) != null)
            {
                return RedirectToAction("AddressAndPayment");
            }
            else
            {
                return RedirectToAction("AddressAndPaymentNoAccount");
            }
        }

        [Authorize]
        public ActionResult AddressAndPayment(CheckOutViewModel model)
        {
            if(model != null)
            {
                ViewBag.message = model.Message;
                if(model.Order != null)
                {
                    return View(model.Order);
                }
            }
            return View();     
        }

        public ActionResult AddressAndPaymentNoAccount(CheckOutViewModel model)
        {
            if (model != null)
            {
                ViewBag.message = model.Message;
                if (model.Order != null)
                {
                    return View(model.Order);
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var CheckOutService = new CheckOutService();
            var CheckOut = CheckOutService.CheckOut(this.HttpContext, values);
            if(CheckOut.Status != "Complete")
            {
                return RedirectToAction("AddressAndPayment", new { model = CheckOut });
            }
            return RedirectToAction("Complete", new { id = CheckOut.Order.OrderId });
        }

        [HttpPost]
        public ActionResult AddressAndPaymentNoAccount(FormCollection values)
        {
            var CheckOutService = new CheckOutService();
            var CheckOut = CheckOutService.CheckOut(this.HttpContext, values);
            if (CheckOut.Status != "Complete")
            {
                return RedirectToAction("AddressAndPaymentNoAccount", new { model = CheckOut });
            }
            return RedirectToAction("Complete", new { id = CheckOut.Order.OrderId });
        }

        public ActionResult Complete(int id)
        {
            bool isValid = db.Order.Any(
                o => o.OrderId == id && o.Username == User.Identity.Name);
            if (isValid)
            {
                return View(id);

            }
            else
            {
                return View("Error");
            }
        }
    }
}