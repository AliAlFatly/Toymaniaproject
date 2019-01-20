using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Toymania.Controllers
{
    public class AccountManagersCController : Controller
    {
        // GET: AccountManagers
        public ActionResult Index()
        {
            return View();
        }

        // GET: AccountManagersC/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountManagersC/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountManagersC/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountManagersC/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountManagersC/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountManagersC/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountManagersC/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
