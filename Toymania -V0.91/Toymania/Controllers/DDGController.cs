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
                        for (int h = 1; h <= 23; h++)
                        {
                            var r = new Random();
                            var min = r.Next(0, 59);
                            var s = r.Next(0, 59);
                            var c = r.Next(0, 20);


                            for (int a = 1; a <= c; a++)
                            {
                                var r1 = r.Next(0, 10);
                                //var u = r.Next(0, ) aantal users
                                var con = this.HttpContext;
                                //var cu = db.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
                                var order = new Order();
                                var OI = new Orders();
                                double w = DateTime.Now.DayOfYear / 7;
                                var week = (int)Math.Ceiling(w);

                                //var U = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
                                //U.balance = cu.balance - t;
                                //d.SaveChanges();

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
                                //order.FirstName = values["FirstName"];
                                //order.LastName = values["LastName"];
                                //order.Address = values["Address"];
                                //order.City = values["City"];
                                //order.State = values["State"];
                                //order.PostalCode = values["PostalCode"];
                                //order.Country = values["Country"];

                                order.Total = 0;
                                
                                //db.Order.Add(order);

                                var D = new DDGController();
                                //D.COD(order, this.HttpContext);
                                db.SaveChanges();



                            }


                        }
                    }
                }


            }





        }

        public void COD(int y, int m, int d, int dayofyear, int min, int h, int s, Order o)
        {
            var date = new DateTime(y, m, d, h, min, s);
            decimal orderTotal = 0;
            Orders O = new Orders();
            var TL = new List<Cart>();

            foreach (var T in TL)
            {
                double w = dayofyear / 7;
                var week = (int)Math.Ceiling(w);

                var OD = new OrderDetails           //alles wat in de cart zit wordt toegevoegd als orderdetails
                {
                    OrderDetailId = O.LastRecordOD() + 1,
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

            //O.ATO(o, c);
            o.Total = orderTotal;
            db.SaveChanges();
        }
    }
}