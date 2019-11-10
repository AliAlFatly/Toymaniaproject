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

namespace Toymania.Controllers
{  
    public class HomeController : Controller
    {
        //private readonly UserManager<ApplicationUser> _userManager;

        //public HomeController(UserManager<ApplicationUser> userManager)
        //{
        //    _userManager = userManager;
        //}
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GetBalance()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var Id = User.Identity.GetUserId();
            var r = db.Users.Find(Id);
            return PartialView("GetBalance.cshtml", r);

            //return View(r);
        }


        public int? LastRecordOrderDetail()
        {
            if(db.OrderDetails.Find(0) != null)
            {
                IQueryable<int> ODIIQ = db.OrderDetails.Select(x => x.OrderDetailId);
                List<int> IL = new List<int> { };
                foreach (int I in ODIIQ)
                {
                    IL.Add(I);
                }

                int o = IL.Last();
                return o;
            }
            else
            {
                return 0;
            }          
        }

        public class top
        {
            public int? c;
            public Toy t;
        }

        public int TenCounts(List<top> t)
        {
            if (t.Count() >= 10)
            {
                return 10;
            }
            else
            {
                return t.Count();
            }
        }

        public List<Toy> TopTenList()
        {
            var r = new List<top>();
            for(int i = 0; i <= 150/*LastRecordOrderDetail()*/; i++)
            {
                if(db.OrderDetails.Find(i) != null && db.OrderDetails.Find(i).Toy.ToysName != "Deleted Toy" /*&& db.OrderDetails.Find(i).year == DateTime.Now.Year*/)
                {
                    if (r.Find(x => x.t.ToysId == db.OrderDetails.Find(i).ToyId) != null)
                    {
                        r.Where(x => x.t.ToysId == db.OrderDetails.Find(i).ToyId).ToList().ForEach(x => x.c += db.OrderDetails.Find(i).Quantity);
                    }
                    else
                    {
                        var TempTop = new top { c = db.OrderDetails.Find(i).Quantity, t = db.OrderDetails.Find(i).Toy };
                        r.Add(TempTop);
                    }
                }
            }

            var Res = r.OrderByDescending(o => o.c).ToList();
            var TopTen = new List<Toy>();
            var TopCount = TenCounts(Res);
            for(int i = 0; i < TopCount; i++)
            {
                TopTen.Add(Res[i].t);
            }

            return TopTen;
        }

        public ActionResult Index()
        {
            //ViewBag.userId = _userManager.GetEmail(HttpContext.User);
            var TopToysList = TopTenList();
            return View(TopToysList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Help()
        {
            ViewBag.Message = "Your Help page";

            return View();
        }
    }
}