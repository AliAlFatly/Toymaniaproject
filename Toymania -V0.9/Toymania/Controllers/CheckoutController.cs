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
        ApplicationDbContext db = new ApplicationDbContext();
        ApplicationDbContext d = new ApplicationDbContext();
        const string PromoCode = "50";

        public ActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            //
            var c = this.HttpContext;
            var s = new ShoppingCart();
            var t = s.GTH(c);
            var cu = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
            if (t < cu.balance)
            {
                var order = new Order();
                var OI = new Orders();
                double w = DateTime.Now.DayOfYear / 7;
                var week = (int)Math.Ceiling(w);

                var U = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
                U.balance = cu.balance - t;
                d.SaveChanges();

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
            }




            //
            //var order = new Order();
            //var OI = new Orders();
            //double w = DateTime.Now.DayOfYear / 7;
            //var week = (int)Math.Ceiling(w);

            ////TryUpdateModel(order);
            //order.OrderId = OI.LastRecordO() + 1;
            //order.Username = User.Identity.Name;
            //order.OrderDate = DateTime.Now;
            //order.year = DateTime.Now.Year;
            //order.month = DateTime.Now.Month;
            //order.day = DateTime.Now.Day;
            //order.minute = DateTime.Now.Minute;
            //order.second = DateTime.Now.Second;
            //order.hour = DateTime.Now.Hour;
            //order.week = week;       
            //order.FirstName = values["FirstName"];
            //order.LastName = values["LastName"];
            //order.Address = values["Address"];
            //order.City = values["City"];
            //order.State = values["State"];
            //order.PostalCode = values["PostalCode"];
            //order.Country = values["Country"];


            //order.Total = 0;

            ////db.Order.Add(order);


            //var cart = ShoppingCart.GC(this.HttpContext);
            //cart.CO(order, this.HttpContext);
            //db.SaveChanges();


            //return RedirectToAction("Complete", new { id = order.OrderId });
            //
            return RedirectToAction("AddressAndPaymentN"); //add diffrent controlere with viewbag message that balance is low


        }

        public ActionResult AddressAndPaymentN()
        {
            //ViewBag.message = "Your balance is low, Please update your balance before completed the purchase or remove some of the items in your cart.";
            return View();
        }

        [HttpPost]
        public ActionResult AddressAndPaymentN(FormCollection values)
        {
            //
            var c = this.HttpContext;
            var s = new ShoppingCart();
            var t = s.GTH(c);
            var cu = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
            if (t < cu.balance)
            {
                var order = new Order();
                var OI = new Orders();
                double w = DateTime.Now.DayOfYear / 7;
                var week = (int)Math.Ceiling(w);

                var U = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
                U.balance = cu.balance - t;
                d.SaveChanges();

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
            }




            //
            //var order = new Order();
            //var OI = new Orders();
            //double w = DateTime.Now.DayOfYear / 7;
            //var week = (int)Math.Ceiling(w);

            ////TryUpdateModel(order);
            //order.OrderId = OI.LastRecordO() + 1;
            //order.Username = User.Identity.Name;
            //order.OrderDate = DateTime.Now;
            //order.year = DateTime.Now.Year;
            //order.month = DateTime.Now.Month;
            //order.day = DateTime.Now.Day;
            //order.minute = DateTime.Now.Minute;
            //order.second = DateTime.Now.Second;
            //order.hour = DateTime.Now.Hour;
            //order.week = week;       
            //order.FirstName = values["FirstName"];
            //order.LastName = values["LastName"];
            //order.Address = values["Address"];
            //order.City = values["City"];
            //order.State = values["State"];
            //order.PostalCode = values["PostalCode"];
            //order.Country = values["Country"];


            //order.Total = 0;

            ////db.Order.Add(order);


            //var cart = ShoppingCart.GC(this.HttpContext);
            //cart.CO(order, this.HttpContext);
            //db.SaveChanges();


            //return RedirectToAction("Complete", new { id = order.OrderId });
            //
            return RedirectToAction("AddressAndPaymentN"); //add diffrent controlere with viewbag message that balance is low


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