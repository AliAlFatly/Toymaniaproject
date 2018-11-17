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
        private TSE15 db = new TSE15();

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
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SCName");
            return View();
        }



        public int LTI()   // last toy id
        {
            IQueryable<int> TILIQ = db.Toy.Select(x => x.ToysId); //recordid list IQueryable
            List<int> RL = new List<int> { };   //record list
            foreach (int LOI in TILIQ)
            {
                RL.Add(LOI);
            }
            //for (int i = 0; i < db.Cart.Single(x=>x.RecordId), i++ ){}

            //var RIL = db.Cart.ToLookup(e => e.RecordId);


            int LR = RL.Last();
            return LR;
        }



        // POST: ToyManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ToysName,CategoryId,ProducerId,ItemArtUrl,ItemArtUrl2,ItemArtUrl3,Price,MinimumAge,SubCategoryId")] Toy toy) //public ActionResult Create([Bind(Include = "ToysId,ToysName,CategoryId,ProducerId,ItemArtUrl,Price,Counter,MinimumAge,SubCategoryId")] Toy toy)
        {
            Toy T = new Toy
            {
                ToysId = LTI() + 1,
                ToysName = toy.ToysName,
                CategoryId = toy.CategoryId,
                ProducerId = toy.ProducerId,
                ItemArtUrl = toy.ItemArtUrl,
                Price = toy.Price,
                Counter = 0,
                MinimumAge = toy.MinimumAge,
                SubCategories = toy.SubCategories

            };

            if (ModelState.IsValid)
            {

                db.Toy.Add(T);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CName", T.CategoryId);
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name", T.ProducerId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SCName", toy.SubCategoryId);
            return View(T);
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
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SCName", toy.SubCategoryId);
            return View(toy);
        }

        // POST: ToyManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ToysId,ToysName,CategoryId,ProducerId,ItemArtUrl,Price,Counter,MinimumAge,SubCategoryId")] Toy toy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CName", toy.CategoryId);
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name", toy.ProducerId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SCName", toy.SubCategoryId);
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
