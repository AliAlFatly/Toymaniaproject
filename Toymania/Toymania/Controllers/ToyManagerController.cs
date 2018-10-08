using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Toymania.Models;

namespace Toymania.Controllers
{
    [Authorize(Users ="AliAlFatly@outlook.com")]
    public class ToyManagerController : Controller
    {
        private TSE7 db = new TSE7();

        // GET: ToyManager
        public ActionResult Index()
        {
            var toy = db.Toy.Include(t => t.Categories).Include(t => t.Producers);
            return View(toy.ToList());
        }

        // GET: ToyManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Toy toy = db.Toy.Find(id);
            if (toy == null)
            {
                return HttpNotFound();
            }
            return View(toy);
        }

        // GET: ToyManager/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CName");
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name");
            return View();
        }

        // POST: ToyManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ToysId,ToysName,CategoryId,ProducerId,ItemArtUrl,Price,Counter,MinimumAge,type")] Toy toy)
        {
            if (ModelState.IsValid)
            {
                db.Toy.Add(toy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CName", toy.CategoryId);
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name", toy.ProducerId);
            return View(toy);
        }

        // GET: ToyManager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Toy toy = db.Toy.Find(id);
            if (toy == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CName", toy.CategoryId);
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name", toy.ProducerId);
            return View(toy);
        }

        // POST: ToyManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ToysId,ToysName,CategoryId,ProducerId,ItemArtUrl,Price,Counter,MinimumAge,type")] Toy toy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CName", toy.CategoryId);
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name", toy.ProducerId);
            return View(toy);
        }

        // GET: ToyManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Toy toy = db.Toy.Find(id);
            if (toy == null)
            {
                return HttpNotFound();
            }
            return View(toy);
        }

        // POST: ToyManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Toy toy = db.Toy.Find(id);
            db.Toy.Remove(toy);
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
