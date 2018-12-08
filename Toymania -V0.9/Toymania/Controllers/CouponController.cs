using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Toymania.Models;
using Toymania.ViewModels;
using Microsoft.AspNet.Identity;


namespace Toymania.Controllers
{
    

    public class CouponController : Controller
    {

        //private ApplicationUserManager c;
        //private readonly ApplicationUserManager _userManager;

        //public HttpContextBase c;

        ApplicationDbContext db = new ApplicationDbContext();
        ApplicationDbContext d = new ApplicationDbContext();
        private const string L = "abcdefghijklmnopqrstuvwxyz";
        private readonly char[] A = (L + L.ToUpper() + "123456789").ToCharArray();


        //public CouponController(HttpContextBase _c)
        //{
        //    c = _c;
        //}

        //public CouponController()
        //{
        //    c = UserManager<ApplicationUser>;
        //}


        public int LID()
        {
            if (db.Coupon.Find(0) == null)
            {
                return 0;
            }
            else
            {
                IQueryable<int> IQ = db.Coupon.Select(x => x.Id); //recordid list IQueryable
                List<int> i = new List<int> { };   //record list
                foreach (int I in IQ)
                {
                    i.Add(I);
                }

                int r = i.Last();
                //if(db.Coupon.Find(r + 1) == null)
                //{
                //    return r + 1;
                //}
                return r + 1;
            }


        }

        // GET: Coupon
        public ActionResult Index()
        {
            return View();
        }

        public string GCoupon(int l)
        {
            StringBuilder r = new StringBuilder();
            Random rn = new Random();
            for(int i = 0; i < l; i++)
            {
                r.Append(A[rn.Next(A.Length)]);
            }
            return r.ToString();
        }

        public ActionResult GenerateCoupons(/*int a,*/ decimal v)
        {
            var len = 16;
            var C = new Coupon
            {
                Id = LID(),
                Code = GCoupon(len),
                Used = false,
                Value = v
            };
            db.Coupon.Add(C);
            db.SaveChanges();

            return View(C);
        }
        
        public ActionResult UB()
        {

            return View();
        }


        public ActionResult UBP(string C/*, HttpContextBase c*/)
        {
            var c = this.HttpContext;
            var Co = db.Coupon.Where(P => P.Code == C).FirstOrDefault();
            //var a = db.Coupon.Find(Co.Id);
            if (Co != null)
            {
                if (!Co.Used)
                {
                    var v = (from co in db.Coupon
                             where co.Code == C
                             select co.Value).ToList().FirstOrDefault();

                    var user = d.Users.SingleOrDefault(n => n.Email == c.User.Identity.Name);
                    var Coupon = db.Coupon.SingleOrDefault(x => x.Code == C);
                    user.balance += v;
                    d.SaveChanges();

                    Coupon.Used = true;
                    db.SaveChanges();

                    var b = (from u in d.Users
                             where c.User.Identity.Name == u.Email
                             select (decimal)u.balance).FirstOrDefault();

                    return View(Coupon);  //make viewmodel with balance info?? and coupon 
                }
                else
                {
                    ViewBag.message = "The Coupon is used";
                    return View(); //error message handling
                }
            }
            else
            {
                ViewBag.message = "Code is incorrect";
                return View(); //error message handling
            }

        }



    }
}