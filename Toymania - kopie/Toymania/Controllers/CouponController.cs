using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Toymania.Models;
using Toymania.ViewModels;

namespace Toymania.Controllers
{
    public class CouponController : Controller
    {
        TSE15 db = new TSE15();
        ApplicationDbContext d = new ApplicationDbContext();
        private const string L = "abcdefghijklmnopqrstuvwxyz";
        private readonly char[] A = (L + L.ToUpper() + "123456789").ToCharArray();

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
         
        public ActionResult UB(HttpContextBase c, string C)
        {
            var Co = db.Coupon.Where(P => P.Code == C).FirstOrDefault();
            if(db.Coupon.Find(Co.Id) != null)
            {
                if (!Co.Used)
                {
                    var v = (from co in db.Coupon
                             where co.Code == C
                             select co.Value).ToList().FirstOrDefault();

                    var user = d.Users.SingleOrDefault(x => x.Email == c.User.Identity.Name);
                    var Coupon = db.Coupon.SingleOrDefault(x => x.Code == C);
                    user.balance += v;
                    d.SaveChanges();

                    Coupon.Used = true;
                    db.SaveChanges();

                    var b = (from u in d.Users
                             where c.User.Identity.Name == u.Email
                             select (decimal)u.balance).FirstOrDefault();

                    return View(b);  //make viewmodel with balance info??
                }
                else
                {
                    return View("code is used"); //error message handling
                }
            }
            else
            {
                return View("Code is incorrect"); //error message handling
            }

        }



    }
}