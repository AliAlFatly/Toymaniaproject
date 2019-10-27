using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Toymania.Models;
using Toymania.ViewModels;

namespace Toymania.Controllers
{
    public class TrackerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tracker
        public ActionResult Index()
        {
            var OIS = db.OrderDetails.Where(d => d.tracker == "In the storage").Include(o => o.Order).Include(o => o.Toy);
            var OS = db.OrderDetails.Where(d => d.tracker == "Being deliverd").Include(o => o.Order).Include(o => o.Toy);
            var OC = db.OrderDetails.Where(d => d.tracker =="Completed").Include(o => o.Order).Include(o => o.Toy);
            var ODVM = new TrackerViewModel
            {
                IS = OIS.ToList(),
                S = OS.ToList(),
                C = OC.ToList()
            };
            return View(ODVM);
        }

        public ActionResult IndexC()
        {
            var OIS = db.OrderDetails.Where(d => d.tracker == "In the storage").Include(o => o.Order).Include(o => o.Toy);
            var OS = db.OrderDetails.Where(d => d.tracker == "Being deliverd").Include(o => o.Order).Include(o => o.Toy);
            var OC = db.OrderDetails.Where(d => d.tracker == "Completed").Include(o => o.Order).Include(o => o.Toy);
            var ODVM = new TrackerViewModel
            {
                IS = OIS.ToList(),
                S = OS.ToList(),
                C = OC.ToList()
            };
            return View(ODVM);
        }


        public ActionResult IndexD()
        {
            var OIS = db.OrderDetails.Where(d => d.tracker == "In the storage").Include(o => o.Order).Include(o => o.Toy);
            var OS = db.OrderDetails.Where(d => d.tracker == "Being deliverd").Include(o => o.Order).Include(o => o.Toy);
            var OC = db.OrderDetails.Where(d => d.tracker == "Completed").Include(o => o.Order).Include(o => o.Toy);
            var ODVM = new TrackerViewModel
            {
                IS = OIS.ToList(),
                S = OS.ToList(),
                C = OC.ToList()
            };
            return View(ODVM);
        }

        // GET: OrderDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            return View(orderDetails);
        }


        public ActionResult C(int? id, string d) //set completed
        {
            OrderDetails OD = db.OrderDetails.Find(id);

            OD.tracker = "Completed";
            OD.Status = "Completed";
            db.SaveChanges();

            if (d == "D")
            {
                return RedirectToAction("IndexD");
            }
            else if (d == "C")
            {
                return RedirectToAction("IndexC");
            }
            else
            {
                return RedirectToAction("Index");
            }
            //return RedirectToAction("IndexD");
        }

        public ActionResult S(int? id, string d) //set in progress
        {
            OrderDetails OD = db.OrderDetails.Find(id);

            OD.tracker = "Being deliverd";
            OD.Status = "In progress";
            db.SaveChanges();

            if (d == "D")
            {
                return RedirectToAction("IndexD");
            }
            else if (d == "C")
            {
                return RedirectToAction("IndexC");
            }
            else
            {
                return RedirectToAction("Index");
            }
            //return RedirectToAction("IndexD");

        }

        public ActionResult IS(int? id, string d) //set in progress
        {
            OrderDetails OD = db.OrderDetails.Find(id);

            OD.tracker = "In the storage";
            OD.Status = "In progress";
            db.SaveChanges();

            if (d == "D")
            {
                return RedirectToAction("IndexD");
            }
            else if (d == "C")
            {
                return RedirectToAction("IndexC");
            }
            else
            {
                return RedirectToAction("Index");
            }
            //return RedirectToAction("IndexD");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
