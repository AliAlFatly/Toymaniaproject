using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toymania.Models;
using Toymania.ViewModels;


namespace Toymania.Controllers
{
    
    public class CheckoutController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        ApplicationDbContext d = new ApplicationDbContext();
        const string PromoCode = "50";


        public ActionResult AddressAndPaymentManager()
        {
            var c = this.HttpContext;
            var s = new ShoppingCart();
            var t = s.GTH(c);
            var cu = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
            var cart = ShoppingCart.GC(this.HttpContext);
            if (d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name) != null)
            {
                return RedirectToAction("AddressAndPayment");
            }
            else
            {
                return RedirectToAction("AddressAndPaymentU");
            }
        }

        [Authorize]
        public ActionResult AddressAndPayment()
        {
            var cart = ShoppingCart.GC(this.HttpContext);
            var s = new ShoppingCart();
            var c = this.HttpContext;
            var t = s.GTH(c);
            if (t < 0.01m)
            {
                return RedirectToAction("Index", "ShoppingCart"); //add message
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            //
            var c = this.HttpContext;
            var s = new ShoppingCart();
            var t = s.GTH(c);
            var cu = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
            var cart = ShoppingCart.GC(this.HttpContext);
            if (values["FirstName"] == "" | values["LastName"] == "" | values["Address"] == "" | values["City"] == "" | values["State"] == "" | values["PostalCode"] == "" | values["Country"] == "")
            {
                return RedirectToAction("uncomplete");
            }

            if (cart.GCT() != null)
            {

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
                    order.OrderId = OI.LastRecordO();
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

                    
                    cart.CO(order, this.HttpContext);
                    db.SaveChanges();


                    return RedirectToAction("Complete", new { id = order.OrderId });
                }
            }
            else
            {
                return RedirectToAction("Index", "ShoppingCart");
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


        public ActionResult AddressAndPaymentU()
        {
            var cart = ShoppingCart.GC(this.HttpContext);
            if (cart.GT() < 0.01m)
            {
                return RedirectToAction("Index", "ShoppingCart"); //add your cart is empty please add items to your cart before proceeding
            }
            else
            {
                return View();
            }            
        }

        [HttpPost]
        public ActionResult AddressAndPaymentU(FormCollection values)
        {
            //
            var c = this.HttpContext;
            var s = new ShoppingCart();
            
            var cu = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
            var C = values["Couponcode"];
            
            var cart = ShoppingCart.GC(this.HttpContext);
            var t = cart.GT();
            if (values["FirstName"] == "" | values["LastName"] == "" | values["Address"] == "" | values["City"] == "" | values["State"] == "" | values["PostalCode"] == "" | values["Country"] == "")
            {
                return RedirectToAction("uncompleteU");
            }
            if (db.Coupon.SingleOrDefault(x => x.Code == C) != null)
            {
                //if (cart.GCT() != null)
                //{
                var Coupon = db.Coupon.SingleOrDefault(x => x.Code == C);
                var a = t < Coupon.Value;

                if (!Coupon.Used)
                {
                    if (t < Coupon.Value)
                    {
                        var order = new Order();
                        var OI = new Orders();
                        double w = DateTime.Now.DayOfYear / 7;
                        var week = (int)Math.Ceiling(w);

                        var U = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
                        Coupon.Value = Coupon.Value - t;
                        //d.SaveChanges();

                        //TryUpdateModel(order);
                        order.OrderId = OI.LastRecordO();
                        order.Username = "Unregistered user";
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


                        cart.CO(order, this.HttpContext);
                        db.SaveChanges();


                        return RedirectToAction("Complete", new { id = order.OrderId });
                    }
                    else
                    {
                        return RedirectToAction("AddressAndPaymentNU");//add diffrent controlere with viewbag message that balance is low
                    }
                }
                else
                {
                    return RedirectToAction("CouponUsed");
                }
                //}
                //else
                //{
                //    return RedirectToAction("Index", "ShoppingCart");
                //}
            }
            else
            {
                return RedirectToAction("CouponDontExist");
            }             
        }


        public ActionResult CouponDontExist()
        {
            var cart = ShoppingCart.GC(this.HttpContext);
            if (cart.GT() < 0.01m)
            {
                return RedirectToAction("Index", "ShoppingCart"); //add your cart is empty please add items to your cart before proceeding
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CouponDontExist(FormCollection values)
        {
            //
            var c = this.HttpContext;
            var s = new ShoppingCart();

            var cu = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
            var C = values["Couponcode"];

            var cart = ShoppingCart.GC(this.HttpContext);
            var t = cart.GT();
            if (values["FirstName"] == "" | values["LastName"] == "" | values["Address"] == "" | values["City"] == "" | values["State"] == "" | values["PostalCode"] == "" | values["Country"] == "")
            {
                return RedirectToAction("uncompleteU");
            }

            if (db.Coupon.SingleOrDefault(x => x.Code == C) != null)
            {
                //if (cart.GCT() != null)
                //{
                var Coupon = db.Coupon.SingleOrDefault(x => x.Code == C);
                var a = t < Coupon.Value;

                if (!Coupon.Used)
                {
                    if (t < Coupon.Value)
                    {
                        var order = new Order();
                        var OI = new Orders();
                        double w = DateTime.Now.DayOfYear / 7;
                        var week = (int)Math.Ceiling(w);

                        var U = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
                        Coupon.Value = Coupon.Value - t;
                        //d.SaveChanges();

                        //TryUpdateModel(order);
                        order.OrderId = OI.LastRecordO();
                        order.Username = "Unregistered user";
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


                        cart.CO(order, this.HttpContext);
                        db.SaveChanges();


                        return RedirectToAction("Complete", new { id = order.OrderId });
                    }
                    else
                    {
                        return RedirectToAction("AddressAndPaymentNU");//add diffrent controlere with viewbag message that balance is low
                    }
                }
                else
                {
                    return RedirectToAction("CouponUsed");
                }
                //}
                //else
                //{
                //    return RedirectToAction("Index", "ShoppingCart");
                //}
            }
            else
            {
                return RedirectToAction("CouponDontExist");
            }
        }

        public ActionResult AddressAndPaymentNU()
        {
            var cart = ShoppingCart.GC(this.HttpContext);
            if (cart.GT() < 0.01m)
            {
                return RedirectToAction("Index", "ShoppingCart"); //add your cart is empty please add items to your cart before proceeding
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddressAndPaymentNU(FormCollection values)
        {
            //
            var c = this.HttpContext;
            var s = new ShoppingCart();

            var cu = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
            var C = values["Couponcode"];

            var cart = ShoppingCart.GC(this.HttpContext);
            var t = cart.GT();
            if (values["FirstName"] == "" | values["LastName"] == "" | values["Address"] == "" | values["City"] == "" | values["State"] == "" | values["PostalCode"] == "" | values["Country"] == "")
            {
                return RedirectToAction("uncompleteU");
            }

            if (db.Coupon.SingleOrDefault(x => x.Code == C) != null)
            {
                //if (cart.GCT() != null)
                //{
                var Coupon = db.Coupon.SingleOrDefault(x => x.Code == C);
                var a = t < Coupon.Value;

                if (!Coupon.Used)
                {
                    if (t < Coupon.Value)
                    {
                        var order = new Order();
                        var OI = new Orders();
                        double w = DateTime.Now.DayOfYear / 7;
                        var week = (int)Math.Ceiling(w);

                        var U = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
                        Coupon.Value = Coupon.Value - t;
                        //d.SaveChanges();

                        //TryUpdateModel(order);
                        order.OrderId = OI.LastRecordO();
                        order.Username = "Unregistered user";
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


                        cart.CO(order, this.HttpContext);
                        db.SaveChanges();


                        return RedirectToAction("Complete", new { id = order.OrderId });
                    }
                    else
                    {
                        return RedirectToAction("AddressAndPaymentNU");//add diffrent controlere with viewbag message that balance is low
                    }
                }
                else
                {
                    return RedirectToAction("CouponUsed");
                }
                //}
                //else
                //{
                //    return RedirectToAction("Index", "ShoppingCart");
                //}
            }
            else
            {
                return RedirectToAction("CouponDontExist");
            }
        }


        public ActionResult CouponUsed()
        {
            var cart = ShoppingCart.GC(this.HttpContext);
            if (cart.GT() < 0.01m)
            {
                return RedirectToAction("Index", "ShoppingCart"); //add your cart is empty please add items to your cart before proceeding
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CouponUsed(FormCollection values)
        {
            //
            var c = this.HttpContext;
            var s = new ShoppingCart();

            var cu = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
            var C = values["Couponcode"];

            var cart = ShoppingCart.GC(this.HttpContext);
            var t = cart.GT();
            if (values["FirstName"] == "" | values["LastName"] == "" | values["Address"] == "" | values["City"] == "" | values["State"] == "" | values["PostalCode"] == "" | values["Country"] == "")
            {
                return RedirectToAction("uncompleteU");
            }

            if (db.Coupon.SingleOrDefault(x => x.Code == C) != null)
            {
                //if (cart.GCT() != null)
                //{
                var Coupon = db.Coupon.SingleOrDefault(x => x.Code == C);
                var a = t < Coupon.Value;

                if (!Coupon.Used)
                {
                    if (t < Coupon.Value)
                    {
                        var order = new Order();
                        var OI = new Orders();
                        double w = DateTime.Now.DayOfYear / 7;
                        var week = (int)Math.Ceiling(w);

                        var U = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
                        Coupon.Value = Coupon.Value - t;
                        //d.SaveChanges();

                        //TryUpdateModel(order);
                        order.OrderId = OI.LastRecordO();
                        order.Username = "Unregistered user";
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


                        cart.CO(order, this.HttpContext);
                        db.SaveChanges();


                        return RedirectToAction("Complete", new { id = order.OrderId });
                    }
                    else
                    {
                        return RedirectToAction("AddressAndPaymentNU");//add diffrent controlere with viewbag message that balance is low
                    }
                }
                else
                {
                    return RedirectToAction("CouponUsed");
                }
                //}
                //else
                //{
                //    return RedirectToAction("Index", "ShoppingCart");
                //}
            }
            else
            {
                return RedirectToAction("CouponDontExist");
            }
        }

        public ActionResult AddressAndPaymentN()
        {
            var cart = ShoppingCart.GC(this.HttpContext);
            var s = new ShoppingCart();
            var c = this.HttpContext;
            var t = s.GTH(c);
            if (t < 0.01m)
            {
                return RedirectToAction("Index", "ShoppingCart"); //add message
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]
        public ActionResult AddressAndPaymentN(FormCollection values)
        {
            //
            var c = this.HttpContext;
            var s = new ShoppingCart();
            var t = s.GTH(c);
            var cu = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
            if (values["FirstName"] == "" | values["LastName"] == "" | values["Address"] == "" | values["City"] == "" | values["State"] == "" | values["PostalCode"] == "" | values["Country"] == "")
            {
                return RedirectToAction("uncomplete");
            }

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
                order.OrderId = OI.LastRecordO();
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

            return RedirectToAction("AddressAndPaymentN"); //add diffrent controlere with viewbag message that balance is low


        }

        public ActionResult uncompleteU()
        {
            var cart = ShoppingCart.GC(this.HttpContext);
            if (cart.GT() < 0.01m)
            {
                return RedirectToAction("Index", "ShoppingCart"); //add your cart is empty please add items to your cart before proceeding
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult uncompleteU(FormCollection values)
        {
            //
            var c = this.HttpContext;
            var s = new ShoppingCart();

            var cu = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
            var C = values["Couponcode"];

            var cart = ShoppingCart.GC(this.HttpContext);
            var t = cart.GT();
            if (values["FirstName"] == "" | values["LastName"] == "" | values["Address"] == "" | values["City"] == "" | values["State"] == "" | values["PostalCode"] == "" | values["Country"] == "")
            {
                return RedirectToAction("uncompleteU");
            }
            if (db.Coupon.SingleOrDefault(x => x.Code == C) != null)
            {
                //if (cart.GCT() != null)
                //{
                var Coupon = db.Coupon.SingleOrDefault(x => x.Code == C);
                var a = t < Coupon.Value;

                if (!Coupon.Used)
                {
                    if (t < Coupon.Value)
                    {
                        var order = new Order();
                        var OI = new Orders();
                        double w = DateTime.Now.DayOfYear / 7;
                        var week = (int)Math.Ceiling(w);

                        var U = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
                        Coupon.Value = Coupon.Value - t;
                        //d.SaveChanges();

                        //TryUpdateModel(order);
                        order.OrderId = OI.LastRecordO();
                        order.Username = "Unregistered user";
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


                        cart.CO(order, this.HttpContext);
                        db.SaveChanges();


                        return RedirectToAction("Complete", new { id = order.OrderId });
                    }
                    else
                    {
                        return RedirectToAction("AddressAndPaymentNU");//add diffrent controlere with viewbag message that balance is low
                    }
                }
                else
                {
                    return RedirectToAction("CouponUsed");
                }
                //}
                //else
                //{
                //    return RedirectToAction("Index", "ShoppingCart");
                //}
            }
            else
            {
                return RedirectToAction("CouponDontExist");
            }
        }

        public ActionResult uncomplete()
        {
            var cart = ShoppingCart.GC(this.HttpContext);
            var s = new ShoppingCart();
            var c = this.HttpContext;
            var t = s.GTH(c);
            if (t < 0.01m)
            {
                return RedirectToAction("Index", "ShoppingCart"); //add message
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult uncomplete(FormCollection values)
        {
            //
            var c = this.HttpContext;
            var s = new ShoppingCart();
            var t = s.GTH(c);
            var cu = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
            var cart = ShoppingCart.GC(this.HttpContext);
            if (values["FirstName"] == "" | values["LastName"] == "" | values["Address"] == "" | values["City"] == "" | values["State"] == "" | values["PostalCode"] == "" | values["Country"] == "")
            {
                return RedirectToAction("uncomplete");
            }

            if (cart.GCT() != null)
            {

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
                    order.OrderId = OI.LastRecordO();
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


                    cart.CO(order, this.HttpContext);
                    db.SaveChanges();


                    return RedirectToAction("Complete", new { id = order.OrderId });
                }
            }
            else
            {
                return RedirectToAction("Index", "ShoppingCart");
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