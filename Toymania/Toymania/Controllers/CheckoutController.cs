using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toymania.Models;
using Toymania.ViewModels;

namespace Toymania.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        TSE7 db = new TSE7();
        const string PromoCode = "FREE";

        public ActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;
                    db.Order.Add(order);
                    db.SaveChanges();
                    var cart = ShoppingCart.GC(this.HttpContext);
                    cart.CO(order);

                    return RedirectToAction("Complete", new { id = order.OrderId });

                }
            }
            catch
            {
                return View(order);
            }

            
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