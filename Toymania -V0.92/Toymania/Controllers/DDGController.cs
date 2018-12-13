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
using System.Globalization;

namespace Toymania.Controllers
{
    public class DDGController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: DDG
        public ActionResult Index()
        {
            DG(2015, 2018);
            return View();
        }

        public void DG(int fy, int ly)
        {
            for (int y = fy; y <= ly; y++)
            {
                var dayofyear = 0;
                for (int m = 1; m <= 12; m++)
                {
                    var days = DateTime.DaysInMonth(y, m);
                    for (int d = 1; d <= days; d++)
                    {
                        dayofyear++;
                        for (int h = 10; h <= 23; h++)
                        {
                            var r = new Random();
                            var min = r.Next(0, 59);
                            var s = r.Next(0, 59);
                            var c = r.Next(1, 4);
                            var c2 = r.Next(1, 3);
                            
                            //cart?
                            var cl = new List<Cart>();
                            for(int TCC = 1; TCC <= c2; TCC++)
                            {
                                var t = r.Next(1, 1036);
                                var TEMPC = new Cart
                                {
                                    RecordId = TCC,
                                    CartId = "toymaniat1@gmail.com",
                                    ToyId = t,
                                    Count = r.Next(1, 8),
                                    DateCreated = new DateTime(y, m, d, h, m, s),
                                    Toy = db.Toy.Find(t)
                                    
                                    
                                };
                                cl.Add(TEMPC);
                            }
                           
                            for (int a = 1; a <= c; a++)
                            {
                                var r1 = r.Next(0, 10);                                
                                var con = this.HttpContext;
                                //var u = db.Users.Find("9653b74e-16fe-4738-b3f9-818e5166e741");
                                var order = new Order();
                                var OI = new Orders();
                                var date = new DateTime(y, m, d, h, m, s);
                                double we = dayofyear / 7;
                                var w = (int)Math.Ceiling(we);
                                var week = Week(dayofyear);
                                if(week == 0)
                                {
                                    week = 1;
                                }
                                var u = "toymaniat1@gmail.com";

                                order.OrderId = OI.LastRecordO();
                                order.Username = u;
                                order.OrderDate = new DateTime(y, m, d, h, m, s);
                                order.year = y;
                                order.month = m;
                                order.day = d;
                                order.minute = min;
                                order.second = s;
                                order.hour = h;
                                order.week = week;
                                order.FirstName = "a";
                                order.LastName = "customer";
                                order.Address = "FVE";
                                order.City = "Rotterdam";
                                order.State = "Rotterdam";
                                order.PostalCode = "1111 XX";
                                order.Country = "Nederland";
                                order.Total = 0;                              
                                //db.Order.Add(order);
                                var D = new DDGController();
                                COD(y, m, d, dayofyear, min, h, s, order, u, cl);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        public void COD(int y, int m, int d, int dayofyear, int min, int h, int s, Order o, string c, List<Cart> TL)
        {
            var date = new DateTime(y, m, d, h, min, s);
            decimal orderTotal = 0;
            Orders O = new Orders();
            //var TL = new List<Cart>();

            foreach (var T in TL)
            {
                //double w = dayofyear / 7;
                //var week = (int)Math.Ceiling(w);
                var week = Week(dayofyear);
                var OD = new OrderDetails           //alles wat in de cart zit wordt toegevoegd als orderdetails
                {
                    //OrderDetailId = O.LastRecordOD(),
                    ToyId = T.ToyId,
                    OrderId = o.OrderId,
                    UnitPrice = T.Toy.Price,
                    Quantity = T.Count,
                    Week = week,
                    Month = m,
                    year = y,
                    Day = d,
                    Hour = h,
                    Minute = min,
                    Status = "Completed",
                    CName = T.Toy.Categories.CName,
                    SCName = T.Toy.SubCategories.SCName
                };

                o.Total += (T.Count * T.Toy.Price);

                db.OrderDetails.Add(OD);
            }

            AO(o, c);
            o.Total = orderTotal;
            db.SaveChanges();
        }

        public void AO(Order O, string c)
        {
            if (O != null && c != null)
            {
                Order o = new Order
                {
                    //OrderId = LastRecordO() + 1,
                    //OrderId = O.OrderId,
                    Username = c,
                    FirstName = O.FirstName,
                    LastName = O.LastName,
                    Address = O.Address,
                    year = O.year,
                    month = O.month,
                    day = O.day,
                    minute = O.minute,
                    second = O.second,
                    hour = O.hour,
                    week = O.week,
                    City = O.City,
                    State = O.State,
                    PostalCode = O.PostalCode,
                    Country = O.Country,
                    //Email = c.User.Identity.Name,
                    Total = O.Total,
                    OrderDate = O.OrderDate,
                };
                db.Order.Add(o);
                db.SaveChanges();


            }
        }

        public int Week(int days)
        {
            var week = 1;

            var d = days - 1;
            if(d == 0)
            {
                return 1;
            }
            else
            {
                var factor = d / 7;

                return week + factor;
            }


        }

    }
}