using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toymania.Models;
using Toymania.ViewModel;

namespace Toymania.Services
{
    public class CheckOutService
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //get context, apply checking
        public CheckOutViewModel CheckOut(HttpContextBase context, FormCollection values)
        {
            //get sessionId and cart.
            var Cart = ShoppingCartService.GetCart(context);
            var TotalPrice = Cart.GetTotalPrice(context);
            var model = new CheckOutViewModel();
            if (values["FirstName"] == "" | values["LastName"] == "" | values["Address"] == "" | values["City"] == "" | values["State"] == "" | values["PostalCode"] == "" | values["Country"] == "")
            {
                model.Message = "Please enter all fields";
                model.Status = "Uncomplete";
                return model;
            }

            if (db.Users.SingleOrDefault(n => n.Email == context.User.Identity.Name) != null)
            {
                model.IsLoggedIn = true;
                var User = db.Users.SingleOrDefault(n => n.Email == context.User.Identity.Name);

                //if cart is not empty
                if (Cart.GetCartToys() != null)
                {
                    //if user have enough balance for the purchase
                    if (TotalPrice < User.balance)
                    {
                        var Order = new Order();
                        var OrderService = new OrdersService();
                        double TempWeek = DateTime.Now.DayOfYear / 7;
                        var week = (int)Math.Ceiling(TempWeek);
                        var user = db.Users.SingleOrDefault(n => n.Email == context.User.Identity.Name);
                        user.balance = User.balance - TotalPrice;
                        //update user balance
                        db.SaveChanges();
                        Order.OrderId = OrderService.LastRecord();
                        Order.Username = context.User.Identity.Name;
                        Order.OrderDate = DateTime.Now;
                        Order.year = DateTime.Now.Year;
                        Order.month = DateTime.Now.Month;
                        Order.day = DateTime.Now.Day;
                        Order.minute = DateTime.Now.Minute;
                        Order.second = DateTime.Now.Second;
                        Order.hour = DateTime.Now.Hour;
                        Order.week = week;
                        Order.FirstName = values["FirstName"];
                        Order.LastName = values["LastName"];
                        Order.Address = values["Address"];
                        Order.City = values["City"];
                        Order.State = values["State"];
                        Order.PostalCode = values["PostalCode"];
                        Order.Country = values["Country"];
                        Order.Total = 0;

                        Cart.CreateOrder(Order, context);
                        db.SaveChanges();

                        model.Message = "Complete";
                        model.Status = "Complete";
                        model.Order = Order;
                        return model;
                    }
                    model.Message = "Not Enough balance on users account";
                    model.Status = "Uncomplete";
                    
                    return model;
                }
                else
                {
                    model.Message = "Cart is empty";
                    model.Status = "Empty Cart";
                    return model;
                }
            }
            else
            {
                model.IsLoggedIn = false;
                var coupon = values["Couponcode"];
                if (db.Coupon.SingleOrDefault(x => x.Code == coupon) != null)
                {
                    var Coupon = db.Coupon.SingleOrDefault(x => x.Code == values["Couponcode"]);
                    if (!Coupon.Used)
                    {
                        if (TotalPrice < Coupon.Value)
                        {
                            //missing order detail and empty cart???
                            var Order = new Order();
                            var OrderService = new OrdersService();
                            double TempWeek = DateTime.Now.DayOfYear / 7;
                            var Week = (int)Math.Ceiling(TempWeek);

                            //updating CouponValue
                            Coupon.Value = Coupon.Value - TotalPrice;
                            db.SaveChanges();
                            var TempId = OrderService.LastRecord();
                            Order.OrderId = TempId;
                            Order.Username = "Unregistered user";
                            Order.OrderDate = DateTime.Now;
                            Order.year = DateTime.Now.Year;
                            Order.month = DateTime.Now.Month;
                            Order.day = DateTime.Now.Day;
                            Order.minute = DateTime.Now.Minute;
                            Order.second = DateTime.Now.Second;
                            Order.hour = DateTime.Now.Hour;
                            Order.week = Week;
                            Order.FirstName = values["FirstName"];
                            Order.LastName = values["LastName"];
                            Order.Address = values["Address"];
                            Order.City = values["City"];
                            Order.State = values["State"];
                            Order.PostalCode = values["PostalCode"];
                            Order.Country = values["Country"];

                            Order.Total = 0;

                            //db.Order.Add(order);


                            Cart.CreateOrder(Order, context);
                            db.SaveChanges();

                            model.Message = "Complete";
                            model.Status = "Complete";
                            model.Order = Order;
                            return model;
                        }
                        else
                        {
                            model.Message = "Coupon doesnt have enough balance.";
                            model.Status = "Uncomplete;";
                            return model;
                        }
                    }
                    else
                    {
                        model.Message = "Coupon is used.";
                        model.Status = "Uncomplete;";
                        return model;
                    }

                }
                else
                {
                    model.Message = "Coupon doesnt exist.";
                    model.Status = "Uncomplete;";
                    return model;
                }
            }
        }

    }
}