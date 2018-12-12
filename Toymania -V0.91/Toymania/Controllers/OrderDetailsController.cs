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
    public class OrderDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OrderDetails
        public ActionResult Index()
        {
            //var orderDetails = db.OrderDetails.Include(o => o.History).Include(o => o.Order).Include(o => o.Toy);
            var OO = db.OrderDetails.Where(d => d.Status == "In Progress").Include(o => o.Order).Include(o => o.Toy);
            var OH = db.OrderDetails.Where(d => d.Status == "Completed").Include(o => o.Order).Include(o => o.Toy);
            var ODVM = new HistoryViewModel
            {
                IPOO = OO.ToList(),
                COO = OH.ToList()
            };
            //return View(orderDetails.ToList());
            return View(ODVM);
        }
       
        public ActionResult IndexC()
        {
            //var orderDetails = db.OrderDetails.Include(o => o.History).Include(o => o.Order).Include(o => o.Toy);
            var OO = db.OrderDetails.Where(d => d.Status == "In Progress").Include(o => o.Order).Include(o => o.Toy);
            var OH = db.OrderDetails.Where(d => d.Status == "Completed").Include(o => o.Order).Include(o => o.Toy);
            var ODVM = new HistoryViewModel
            {
                IPOO = OO.ToList(),
                COO = OH.ToList()
            };
            //return View(orderDetails.ToList());
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

        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            //ViewBag.HistoryId = new SelectList(db.History, "HistoryId", "Email");
            ViewBag.OrderId = new SelectList(db.Order, "OrderId", "Username");
            ViewBag.ToyId = new SelectList(db.Toy, "ToysId", "ToysName");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderDetailId,OrderId,ToyId,Quantity,UnitPrice,Status,HistoryId")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.HistoryId = new SelectList(db.History, "HistoryId", "Email", orderDetails.HistoryId);
            ViewBag.OrderId = new SelectList(db.Order, "OrderId", "Username", orderDetails.OrderId);
            ViewBag.ToyId = new SelectList(db.Toy, "ToysId", "ToysName", orderDetails.ToyId);
            return View(orderDetails);
        }

        // GET: OrderDetails/Edit/5
        public ActionResult Edit(int? id)
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
            //ViewBag.HistoryId = new SelectList(db.History, "HistoryId", "Email", orderDetails.HistoryId);
            ViewBag.OrderId = new SelectList(db.Order, "OrderId", "Username", orderDetails.OrderId);
            ViewBag.ToyId = new SelectList(db.Toy, "ToysId", "ToysName", orderDetails.ToyId);
            return View(orderDetails);
        }

        public ActionResult SC(int? id) //set completed
        {
            OrderDetails OD = db.OrderDetails.Find(id);

            OD.Status = "Completed";
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult SIP(int? id) //set in progress
        {
            OrderDetails OD = db.OrderDetails.Find(id);

            OD.Status = "In Progress";
            db.SaveChanges();

            return RedirectToAction("IndexC");
            //return View();
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderDetailId,OrderId,ToyId,Quantity,UnitPrice,Status,HistoryId")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.HistoryId = new SelectList(db.History, "HistoryId", "Email", orderDetails.HistoryId);
            ViewBag.OrderId = new SelectList(db.Order, "OrderId", "Username", orderDetails.OrderId);
            ViewBag.ToyId = new SelectList(db.Toy, "ToysId", "ToysName", orderDetails.ToyId);
            return View(orderDetails);
        }

        // GET: OrderDetails/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetails);
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
    }
}
