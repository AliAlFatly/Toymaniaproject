using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Toymania.Models;
using Toymania.Models;

namespace Toymania.Controllers
{
    public class StoreController : Controller
    {
        // GET: store
        TSE7 db = new TSE7();
        public ActionResult Index()
        {

            //List<Categories> categories = new List<Categories>();
            //Categories category = new Categories();

            //Categories Category0 = db.Categories.SingleOrDefault(x => x.CategoryId == 0);
            //categories.Add(Category0);
            List<Categories> Category = db.Categories.ToList();

            Categories cvm = new Categories();

            List<Categories> cvmList = Category.Select(x => new Categories
            {
                CategoryId = x.CategoryId,
                CName = x.CName,
                Description = x.Description,
                Toy = x.Toy
            }).ToList();

            return View(cvmList);
        }


        public ActionResult Browse(string Category)
        {

            //string message = HttpUtility.HtmlEncode(" a "+Catagory);
            //var catagorymodel = new Category { Cname = Catagory };
            var categorymodel = db.Categories.Include("Toy")
                .Single(c => c.CName == Category);

            //var categorymodel = new CategoriesViewModel { Cname = Category };
            return View(categorymodel);
        }

        public ActionResult Details(int id)
        {

            var toy = db.Toy.Find(id);
            return View(toy);
        }
        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {
            var category = db.Categories.ToList();
            return PartialView(category);
        }



        //public ActionResult Item()
        //{
        //    TSE db = new TSE();

        //    List<Toy> toys = db.Toy.ToList();

        //    ToyViewModel tvm = new ToyViewModel();

        //    List<ToyViewModel> tvmList = toys.Select(x => new ToyViewModel
        //    {
        //        ToysId = x.ToysId,
        //        ToysName = x.ToysName,
        //        CategoryId = x.CategoryId,
        //        Categories = x.Categories,
        //        Category = x.Category,
        //        Counter = x.Counter,
        //        ItemArtUrl = x.ItemArtUrl,
        //        MinimumAge = x.MinimumAge,
        //        Price = x.Price,
        //        Producer = x.Producer,
        //        ProducerId = x.ProducerId,
        //        Producers = x.Producers,
        //        type = x.type

        //    }).ToList();


        //    return View(tvmList);
        //}


    }
}