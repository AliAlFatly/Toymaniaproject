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
            var r = new AccountManageViewModel
            {
                Customer = db.Users.Where(c => c.Role == "Customer").ToList(),
                Admin = db.Users.Where(a => a.Role == "Admin").ToList()
            };
            return View(r /*db.Users.ToList()*/);
            
        }

        public ActionResult IndexA()
        {
            var r = new AccountManageViewModel
            {
                Customer = db.Users.Where(c => c.Role == "Customer").ToList(),
                Admin = db.Users.Where(a => a.Role == "Admin").ToList()
            };
            return View(r /*db.Users.ToList()*/);

        }

        public ActionResult IndexAd()
        {
            var r = new AccountManageViewModel
            {
                Customer = db.Users.Where(c => c.Role == "Customer").ToList(),
                Admin = db.Users.Where(a => a.Role == "Admin").ToList()
            };
            return View(r /*db.Users.ToList()*/);

        }

        public ActionResult IndexAdA()
        {
            var r = new AccountManageViewModel
            {
                Customer = db.Users.Where(c => c.Role == "Customer").ToList(),
                Admin = db.Users.Where(a => a.Role == "Admin").ToList()
            };
            return View(r /*db.Users.ToList()*/);

        }

        public ActionResult SetUserCustomer(string id) 
        {
            var u = db.Users.Find(id);
            u.Role = "Customer";
            db.SaveChanges();
            return RedirectToAction("IndexA");
        }

        public ActionResult SetUserAdmin(string id)
        {
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
                var cur = db.Users.Where(a => a.Email == User.Identity.Name).ToList().First().Role;
                if (cur == "Owner")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("IndexAd");
                }
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
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName, balance, Role")] ApplicationUser applicationUser) //        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationUser).State = EntityState.Modified;
                db.SaveChanges();
                var cur = db.Users.Where(a => a.Email == User.Identity.Name).ToList().First().Role;
                if (cur == "Owner")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("IndexAd");
                }
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
            var cur = db.Users.Where(a => a.Email == User.Identity.Name).ToList().First().Role;
            if (cur == "Owner")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("IndexAd");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Ban(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var u = db.Users.Find(Id);
            if (u == null)
            {
                return HttpNotFound();
            }
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ban([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName, balance, Role")] ApplicationUser applicationUser) //        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (applicationUser.LockoutEndDateUtc > DateTime.Now)
                    {
                        db.Entry(applicationUser).State = EntityState.Modified;
                        db.SaveChanges();
                        var cur = db.Users.Where(a => a.Email == User.Identity.Name).ToList().First().Role;
                        if (cur == "Owner")
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return RedirectToAction("IndexAd");
                        }
                    }
                    else
                    {                       
                        return RedirectToAction("OldDate", new { Id = applicationUser.Id });
                    }

                }
                return View(applicationUser);
            }
            catch
            {
                return RedirectToAction("InvalidDate", new { Id = applicationUser.Id });
            }

        }

        public ActionResult OldDate(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var u = db.Users.Find(Id);
            if (u == null)
            {
                return HttpNotFound();
            }
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OldDate([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName, balance, Role")] ApplicationUser applicationUser) //        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (applicationUser.LockoutEndDateUtc > DateTime.Now)
                    {
                        db.Entry(applicationUser).State = EntityState.Modified;
                        db.SaveChanges();
                        var cur = db.Users.Where(a => a.Email == User.Identity.Name).ToList().First().Role;
                        if (cur == "Owner")
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return RedirectToAction("IndexAd");
                        }
                    }
                    else
                    {
                        return RedirectToAction("OldDate", new { Id = applicationUser.Id });
                    }

                }
                return View(applicationUser);
            }
            catch
            {
                return RedirectToAction("InvalidDate", new { Id = applicationUser.Id });
            }

        }


        public ActionResult InvalidDate(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var u = db.Users.Find(Id);
            if (u == null)
            {
                return HttpNotFound();
            }
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InvalidDate([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName, balance, Role")] ApplicationUser applicationUser) //        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (applicationUser.LockoutEndDateUtc > DateTime.Now)
                    {
                        db.Entry(applicationUser).State = EntityState.Modified;
                        db.SaveChanges();
                        var cur = db.Users.Where(a => a.Email == User.Identity.Name).ToList().First().Role;
                        if (cur == "Owner")
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return RedirectToAction("IndexAd");
                        }
                    }
                    else
                    {
                        return RedirectToAction("OldDate", new { Id = applicationUser.Id });
                    }

                }
                return View(applicationUser);
            }
            catch
            {
                return RedirectToAction("InvalidDate", new { Id = applicationUser.Id });
            }

        }

        public ActionResult IncorrectBan(int id, DateTime d)
        {
            ViewBag.id = id;
            ViewBag.day = d.Day;
            ViewBag.month = d.Month;
            ViewBag.year = d.Year;
            return View();
        }

        public ActionResult unban(string Id)
        {
            var cur = db.Users.Where(a => a.Email == User.Identity.Name).ToList().First().Role;
            if (Id != null & db.Users.Find(Id) != null)
            {

                var u = db.Users.Find(Id);
                u.LockoutEndDateUtc = null;
                db.SaveChanges();

                if(cur == "Owner")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("IndexAd");
                }

            }

            if (cur == "Owner")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("IndexAd");
            }
        }

        //voeg unban toe
        //voeg tappedpage unbanned users
        //stats verbeteren?
    }
}
