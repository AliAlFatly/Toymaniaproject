using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Toymania.Models;
using Toymania.Models;
using Toymania.ViewModels;


namespace Toymania.Controllers
{
    public class StoreController : Controller
    {
        // GET: store
        TSE15 db = new TSE15();
      
        public ActionResult Index(string C, string SC, int? PC, int? P)
        {

            List<Categories> CL = db.Categories.ToList();

            //var product = db.Toy.OrderByDescending(x => x.ToysId).toPageList(1, 10);

            if (C != null)
            {
                ViewBag.Category = C;
                ViewData["Category"] = C;
            }
            if(SC != null)
            {
                ViewBag.SubCategory = SC;
                ViewData["SubCategory"] = SC;
            }
            if(PC != null)
            {
                ViewBag.PriceCategory = PC;
                ViewData["PriceCategory"] = PC;
            }
            if(P != null)
            {
                ViewBag.Page = P;
                ViewData["Page"] = P;
            }
            

            if (C != null && SC == null && PC == null)  //category page 2
            {
                Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category

                BrowsePartial a = new BrowsePartial
                {
                    C = CL,
                    T = OC.Toy,
                    SC = OC.SubCategories,
                    PC = null
                };
                return View(a);
            }
            else if(C != null && PC == null) // subcategory page
            {
                Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                BrowsePartial a = new BrowsePartial
                {
                    C = CL,
                    T = OSC.Toy,
                    SC = OC.SubCategories,
                    PC = null
                };
                return View(a);
            }
            else if(C != null && SC != null && PC != null) // price page naar subcategory filter
            {
                //ICollection<Toy> toy = null;

                if (PC == 1)
                {
                    IQueryable <ICollection<Toy>> TTemp = from TT in db.Toy
                                where TT.SubCategories.SCName == SC && TT.Price > 0.00m && TT.Price < 10.00m
                                select (ICollection<Toy>)TT;
                    ICollection<Toy> TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy,
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 2)
                {
                    IQueryable <ICollection<Toy>> TTemp = from TT in db.Toy
                                where TT.SubCategories.SCName == SC && TT.Price > 9.99m && TT.Price < 20.00m
                                select (ICollection<Toy>)TT;
                    ICollection<Toy> TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy,
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 3)
                {
                    IQueryable <ICollection<Toy>> TTemp = from TT in db.Toy
                                where TT.SubCategories.SCName == SC && TT.Price > 19.99m && TT.Price < 50.00m
                                select (ICollection<Toy>)TT;
                    ICollection<Toy> TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy,
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 4)
                {
                    IQueryable <ICollection<Toy>> TTemp = from TT in db.Toy
                                where TT.SubCategories.SCName == SC && TT.Price > 49.99m && TT.Price < 100.00m
                                select (ICollection<Toy>)TT;
                    ICollection<Toy> TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy,
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 5)
                {
                    IQueryable <ICollection<Toy>> TTemp = from TT in db.Toy
                                where TT.SubCategories.SCName == SC && TT.Price > 99.99m && TT.Price < 500.00m
                                select (ICollection<Toy>)TT;
                    ICollection<Toy> TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy,
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 6)
                {
                    IQueryable <ICollection<Toy>> TTemp = from TT in db.Toy
                                where TT.SubCategories.SCName == SC && TT.Price > 500.00m
                                select (ICollection<Toy>)TT;
                    ICollection<Toy> TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy,
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else
                {
                    return View();
                }
            }
            else if (C != null && PC != null) // price page naar category filter
            {
                //ICollection<Toy> toy = null;

                if (PC == 1)
                {
                    IQueryable <ICollection<Toy>> TTemp = from TT in db.Toy
                                where TT.Categories.CName == C && TT.Price > 0.00m && TT.Price < 10.00m
                                select (ICollection<Toy>)TT;
                    ICollection<Toy> TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy,
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 2)
                {
                    IQueryable <ICollection<Toy>> TTemp = from TT in db.Toy
                                where TT.Categories.CName == C && TT.Price > 9.99m && TT.Price < 20.00m
                                select (ICollection<Toy>)TT;
                    ICollection<Toy> TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy,
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 3)
                {
                    IQueryable <ICollection<Toy>> TTemp = from TT in db.Toy
                                where TT.Categories.CName == C && TT.Price > 19.99m && TT.Price < 50.00m
                                select (ICollection<Toy>)TT;
                    ICollection<Toy> TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy,
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 4)
                {
                    IQueryable <ICollection<Toy>> TTemp = from TT in db.Toy
                                where TT.Categories.CName == C && TT.Price > 49.99m && TT.Price < 100.00m
                                select (ICollection<Toy>)TT;
                    ICollection<Toy> TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy,
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 5)
                {
                    IQueryable <ICollection<Toy>> TTemp = from TT in db.Toy
                                where TT.Categories.CName == C && TT.Price > 99.99m && TT.Price < 500.00m
                                select (ICollection<Toy>)TT;
                    ICollection<Toy> TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy,
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 6)
                {
                    IQueryable <ICollection<Toy>> TTemp = from TT in db.Toy
                                where TT.Categories.CName == C && TT.Price > 500.00m
                                select (ICollection<Toy>)TT;
                    ICollection<Toy> TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy,
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else
                {
                    return View();
                }
            }
            else //start page
            {
                List<Toy> Toys = db.Toy.ToList();
                BrowsePartial a = new BrowsePartial
                {
                    C = CL,
                    T = Toys,
                    SC = null,
                    PC = null,
                };
                return View(a);
            }

            //var SCM = db.SubCategories.Include("Toy").SingleOrDefault(c => c.SCName == SC);

            //var CID = SCM.CategoryId;
            //var CM = db.Categories.SingleOrDefault(c => c.CategoryId == CID);

            //BrowsePartial a = new BrowsePartial
            //{
            //    C = CL,
            //    T = SCM.Toy,
            //    SC = CM.SubCategories,
            //    PC = PC
            //};


            //Categories cvm = new Categories();
            //List<Categories> cvmList = CL.Select(x => new Categories
            //{
            //    CategoryId = x.CategoryId,
            //    CName = x.CName,
            //    Description = x.Description,
            //    Toy = x.Toy
            //}).ToList();

            
        }

        public ActionResult Details(int id)
        {
            ViewData["Tid"] = id;

            Toy toy = db.Toy.Find(id);
            var t = from T in db.Toy
                    where T.ToysId == id
                    select T ;
            Toy TT = t.First();
            return View(TT);
        }

        public ActionResult IndexR(string SRCH)
        {
            List<Categories> CL = db.Categories.ToList();
            List<Toy> Toys = db.Toy.ToList();
            if (SRCH != "" | SRCH != null)
            {
                Toys = db.Toy.Where(x => x.ToysName.Contains(SRCH)).ToList();
            }
            BrowsePartial a = new BrowsePartial
            {
                C = CL,
                T = Toys,
                SC = null,
                PC = null,
            };
            return View(a);
        }
    }
}