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
        TSE15 db = new TSE15();
        const string PromoCode = "50";

        public ActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            var OI = new Orders();
            double w = DateTime.Now.DayOfYear / 7;
            var week = (int)Math.Ceiling(w);

            //TryUpdateModel(order);
            order.OrderId = OI.LastRecordO() + 1;
            order.Username = User.Identity.Name;
            order.OrderDate = DateTime.Now;
            order.year = DateTime.Now.Year;
            order.month = DateTime.Now.Month;
            order.day = DateTime.Now.Day;
            order.minute = DateTime.Now.Minute;
            order.second = DateTime.Now.Second;
            order.hour = DateTime.Now.Hour;
            order.week = week;       
            order.FirstName = values["FirstName"];
            order.LastName = values["LastName"];
            order.Address = values["Address"];
            order.City = values["City"];
            order.State = values["State"];
            order.PostalCode = values["PostalCode"];
            order.Country = values["Country"];
            

            order.Total = 0;

            //db.Order.Add(order);
            

            var cart = ShoppingCart.GC(this.HttpContext);
            cart.CO(order, this.HttpContext);
            db.SaveChanges();


            return RedirectToAction("Complete", new { id = order.OrderId });

            //try
            //{
            //    //if (string.Equals(values["PromoCode"], PromoCode,StringComparison.OrdinalIgnoreCase) == false)
            //    //{
            //    //    return View(order);
            //    //}
            //    //else
            //    //{
            //        order.Username = User.Identity.Name;
            //        order.OrderDate = DateTime.Now;

            //        db.Order.Add(order);
            //        db.SaveChanges();

            //        var cart = ShoppingCart.GC(this.HttpContext);
            //        cart.CO(order, this.HttpContext);

            //        return RedirectToAction("Complete", new { id = order.OrderId });

            //    //}
            //}
            //catch
            //{
            //    return View(order);
            //}

            
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