using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Toymania.Models;
using Toymania.Models;
using Toymania.ViewModels;
using PagedList;
using PagedList.Mvc;
using Toymania.Services;

namespace Toymania.Controllers
{
    public class StoreController : Controller
    {
        // GET: store
        ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index(string category, string subCategory, int? minPrice, int? maxPrice, string search, string pageSize, int? currentPage)
        {
            var StoreService = new StoreService();


            var PageSize = pageSize != null ? Convert.ToInt32(pageSize) : 30;
            var Deleted = "Deleted Toy";
            List<Categories> CategoryList = db.Categories.ToList();
            var MaxPrice = maxPrice ?? (from ToysTable in db.Toy select ToysTable.Price).Max();
            var MinPrice = minPrice ?? 0;


            var Toys = StoreService.GetProducts(category, subCategory, minPrice, maxPrice, search);

            BrowsePartial data = new BrowsePartial
            {
                SelectedCategory = category ?? null,
                SelectedSubCategory = subCategory ?? null,

                MinPrice = MinPrice,
                MaxPrice = (int)Math.Ceiling(MaxPrice),
                Search = search ?? null,
                PageSize = PageSize,

                Categories = CategoryList,
                Toys = Toys.Where(t => t.ToysName != Deleted).ToPagedList(currentPage ?? 1, PageSize),
                SubCategories = (category != null) ? (db.Categories.Include("Toy").Single(c => c.CategoryName == category).SubCategories).ToList() : null,
            };
            return View(data);

        }



        public int? LastRecord()
        {
            if (db.OrderDetails.Find(0) != null)
            {
                IQueryable<int> OrderDetailIdIQeuryable = db.OrderDetails.Select(x => x.OrderDetailId);
                List<int> OrderDetailIdList = new List<int> { };
                foreach (int I in OrderDetailIdIQeuryable)
                {
                    OrderDetailIdList.Add(I);
                }

                int Id = OrderDetailIdList.Last();
                return Id;
            }
            else
            {
                return 0;
            }       
        }

        public class top
        {
            public int? Count;
            public Toy Toy;
        }

        public int TopAmount(List<top> t)
        {
            if (t.Count() >= 6)
            {
                return 6;
            }
            else
            {
                return t.Count();
            }
        }

        public List<Toy> TopTenList() //Top Ten List
        {
            var r = new List<top>();
            for (int i = 0; i <= LastRecord(); i++)
            {
                if (db.OrderDetails.Find(i) != null && db.OrderDetails.Find(i).Toy.ToysName != "Deleted Toy")
                {
                    if (r.Find(x => x.Toy.ToysId == db.OrderDetails.Find(i).ToyId) != null)
                    {
                        r.Where(x => x.Toy.ToysId == db.OrderDetails.Find(i).ToyId).ToList().ForEach(x => x.Count += db.OrderDetails.Find(i).Quantity);
                    }
                    else
                    {
                        var TempTop = new top { Count = db.OrderDetails.Find(i).Quantity, Toy = db.OrderDetails.Find(i).Toy };
                        r.Add(TempTop);
                    }
                }
            }

            var Res = r.OrderByDescending(o => o.Count).ToList();
            var TopsList = new List<Toy>();
            var tp = TopAmount(Res);
            for (int i = 0; i < tp; i++)
            {
                TopsList.Add(Res[i].Toy);
            }

            return TopsList;
        }

        public List<Toy> GetTops(int id)
        {
            var OrderDetails = db.OrderDetails.Where(t => t.ToyId == id).ToList();
            var Orders = new List<int?>();
            foreach (var i in OrderDetails)
            {
                if (i.OrderId != null)
                {
                    if (Orders.Find(x => x.Value == i.OrderId) == null)
                    {
                        Orders.Add(i.OrderId);
                    }

                }
            }

            var r = new List<top>();
            for (int i = 0; i <= 1000/*LROD()*/; i++)
            {
                if (db.OrderDetails.Find(i) != null && db.OrderDetails.Find(i).Toy.ToysName != "Deleted Toy" && db.OrderDetails.Find(i).Toy.ToysId != id)
                {
                    var c = false;
                    foreach (var cc in Orders)
                    {
                        if (db.OrderDetails.Find(i).OrderId == cc)
                        {
                            c = true;
                        }
                    }

                    if (c)
                    {
                        if (r.Find(x => x.Toy.ToysId == db.OrderDetails.Find(i).ToyId) != null)
                        {
                            r.Where(x => x.Toy.ToysId == db.OrderDetails.Find(i).ToyId).ToList().ForEach(x => x.Count += db.OrderDetails.Find(i).Quantity);
                        }
                        else
                        {
                            var TempTop = new top { Count = db.OrderDetails.Find(i).Quantity, Toy = db.OrderDetails.Find(i).Toy };
                            r.Add(TempTop);
                        }
                    }
                }
            }

            var Res = r.OrderByDescending(o => o.Count).ToList();
            var TopsList = new List<Toy>();
            var tp = TopAmount(Res);
            for (int j = 0; j < tp; j++)
            {
                TopsList.Add(Res[j].Toy);
            }

            return TopsList;


        }

        public ActionResult Details(int id)
        {
            ViewData["Tid"] = id;
            //functie krijgt toysid => zoekt alle orderdetails met die toysid => creert order lijst waar orderdetails gemaakt zijn => selecteert de top 5 toys van die orders
            //viewmodel voor toy en toysid
            var Tops = GetTops(id);
            var Model = new Recommended
            {
                Toy = (from T in db.Toy
                       where T.ToysId == id
                       select T).First(),
                RecommendedToys = Tops
            };
            return View(Model);
        }

    }
}