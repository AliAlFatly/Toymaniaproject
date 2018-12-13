using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Toymania.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Toymania.ViewModels;

namespace Toymania.Controllers
{
    
    public class ApplicationUsersController : Controller
    {
        //private ApplicationSignInManager _signInManager;
        //private ApplicationUserManager _userManager;

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationUsers
        public ActionResult Index()
        {
            var dbC = db.Users.Where(c => c.Role == "Customer").ToList();
            var dbA = db.Users.Where(a => a.Role == "Admin").ToList();
            var r = new AccountManageViewModel
            {
                c = dbC,
                a = dbA
            };
            return View(r /*db.Users.ToList()*/);
            
        }

        public ActionResult IndexA()
        {
            var dbC = db.Users.Where(c => c.Role == "Customer").ToList();
            var dbA = db.Users.Where(a => a.Role == "Admin").ToList();
            var r = new AccountManageViewModel
            {
                c = dbC,
                a = dbA
            };
            return View(r /*db.Users.ToList()*/);

        }

        public ActionResult SUC(string id) //set completed
        {
            //var u = db.Users.SingleOrDefault(c => c.Email == id);

            var u = db.Users.Find(id);
            u.Role = "Customer";
            db.SaveChanges();

            return RedirectToAction("IndexA");
        }

        public ActionResult SUA(string id) //set completed
        {
            //var u = db.Users.SingleOrDefault(c => c.Email == id);

            var u = db.Users.Find(id);
            u.Role = "Admin";
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(applicationUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName, balance")] ApplicationUser applicationUser) //        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Banner(int id, DateTime d)
        {
            var u =  db.Users.Find(id);
            DateTime t;
            if(DateTime.TryParse(d.ToString(), out t))
            {
                var lockoutdate = new DateTime(d.Year, d.Month, d.Day, 23, 59, 59);
                u.LockoutEndDateUtc = lockoutdate;
                db.SaveChanges();
                return View();
            }
            else
            {
                RedirectToAction("", new { id = id, d = d });
            }
            return RedirectToAction("", new { id = id, d = d });
         }

        public ActionResult IncorrectBan(int id, DateTime d)
        {

            return View();
        }
    }
}
