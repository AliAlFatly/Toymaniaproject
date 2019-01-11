using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toymania.Models;

namespace Toymania.Controllers
{
    public class ArchiverController : Controller
    {
        // GET: Archiver
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            archiver();
            return RedirectToAction("Index", "Home");

        }

        public void archiver()
        {
            var t = db.Toy.Where(T => T.ToysName != "Deleted toy").ToList();
            foreach(var i in t)
            {
                i.Archive = i.ToysName;
                db.SaveChanges();
            }
        }
    }
}