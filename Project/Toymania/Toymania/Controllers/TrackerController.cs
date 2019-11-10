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
            //add accessable value to viewmodel to set default tab.
            var Model = new TrackerViewModel
            {
                InStorage = db.OrderDetails.Where(d => d.tracker == "In the storage").Include(o => o.Order).Include(o => o.Toy).ToList(),
                Delivering = db.OrderDetails.Where(d => d.tracker == "Being deliverd").Include(o => o.Order).Include(o => o.Toy).ToList(),
                Completed = db.OrderDetails.Where(d => d.tracker == "Completed").Include(o => o.Order).Include(o => o.Toy).ToList()
            };
            return View(Model);
        }

        public ActionResult IndexC()
        {
            var Model = new TrackerViewModel
            {
                InStorage = db.OrderDetails.Where(d => d.tracker == "In the storage").Include(o => o.Order).Include(o => o.Toy).ToList(),
                Delivering = db.OrderDetails.Where(d => d.tracker == "Being deliverd").Include(o => o.Order).Include(o => o.Toy).ToList(),
                Completed = db.OrderDetails.Where(d => d.tracker == "Completed").Include(o => o.Order).Include(o => o.Toy).ToList()
            };
            return View(Model);
        }


        public ActionResult IndexD()
        {
            var Model = new TrackerViewModel
            {
                InStorage = db.OrderDetails.Where(d => d.tracker == "In the storage").Include(o => o.Order).Include(o => o.Toy).ToList(),
                Delivering = db.OrderDetails.Where(d => d.tracker == "Being deliverd").Include(o => o.Order).Include(o => o.Toy).ToList(),
                Completed = db.OrderDetails.Where(d => d.tracker == "Completed").Include(o => o.Order).Include(o => o.Toy).ToList()
            };
            return View(Model);
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


        public ActionResult SetCompleted(int? id, string d) //set completed
        {
            OrderDetails OrderDetail = db.OrderDetails.Find(id);

            OrderDetail.tracker = "Completed";
            OrderDetail.Status = "Completed";
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

        public ActionResult SetBeingDeliverd(int? id, string d) //set in progress
        {
            OrderDetails OrderDetail = db.OrderDetails.Find(id);

            OrderDetail.tracker = "Being deliverd";
            OrderDetail.Status = "In progress";
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

        public ActionResult SetInProgress(int? id, string d) //set in progress
        {
            OrderDetails OrderDetail = db.OrderDetails.Find(id);

            OrderDetail.tracker = "In the storage";
            OrderDetail.Status = "In progress";
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
