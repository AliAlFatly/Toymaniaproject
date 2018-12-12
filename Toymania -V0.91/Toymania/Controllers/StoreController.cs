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


namespace Toymania.Controllers
{
    public class StoreController : Controller
    {
        // GET: store
        ApplicationDbContext db = new ApplicationDbContext();
      
        public ActionResult Index(string C, string SC, int? PC, int? P, string search, string ps)
        {
            //var PS = 3;
            var PS = 30;

            List<Categories> CL = db.Categories.ToList();
            //functie schrijven die prijs category als parameter krijgt en de prijsrange terug geeft om code te verminderen

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
            if(search != null)
            {
                ViewBag.srch = search;
                ViewData["srch"] = search;
            }
            if (ps != null)
            {
                PS = Convert.ToInt32(ps);
                ViewBag.PS = Convert.ToInt32(ps); ;
                ViewData["PageSize"] = Convert.ToInt32(ps); ;
            }


            if (C != null && SC == null && PC == null && search == null)  //category page 2
            {
                Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category

                BrowsePartial a = new BrowsePartial
                {
                    C = CL,
                    T = OC.Toy.ToPagedList(P ?? 1, PS),
                    SC = OC.SubCategories,
                    PC = null
                };
                return View(a);
            }


            else if(C != null && SC != null && PC == null && search == null) // subcategory page
            {
                Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                BrowsePartial a = new BrowsePartial
                {
                    C = CL,
                    T = OSC.Toy.ToPagedList(P ?? 1, PS),
                    SC = OC.SubCategories,
                    PC = null
                };
                return View(a);
            }


            else if(C != null && SC != null && PC != null && search == null) // price page naar subcategory filter
            {
                //ICollection<Toy> toy = null;

                if (PC == 1)
                {
                    var TTemp = from TT in db.Toy
                                where TT.SubCategories.SCName == SC && TT.Price > 0.00m && TT.Price < 10.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach(Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 2)
                {
                    var TTemp = from TT in db.Toy
                                where TT.SubCategories.SCName == SC && TT.Price > 9.99m && TT.Price < 20.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 3)
                {
                    var TTemp = from TT in db.Toy
                                where TT.SubCategories.SCName == SC && TT.Price > 19.99m && TT.Price < 50.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 4)
                {
                    var TTemp = from TT in db.Toy
                                where TT.SubCategories.SCName == SC && TT.Price > 49.99m && TT.Price < 100.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 5)
                {
                    var TTemp = from TT in db.Toy
                                where TT.SubCategories.SCName == SC && TT.Price > 99.99m && TT.Price < 500.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 6)
                {
                    var TTemp = from TT in db.Toy
                                where TT.SubCategories.SCName == SC && TT.Price > 500.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else
                {
                    return View();
                }
            } //X


            else if (C != null && SC == null && PC != null && search == null) // price page naar category filter
            {
                //ICollection<Toy> toy = null;

                if (PC == 1)
                {
                    var TTemp = from TT in db.Toy
                                where TT.Categories.CName == C && TT.Price > 0.00m && TT.Price < 10.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    //SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 2)
                {
                    var TTemp = from TT in db.Toy
                                where TT.Categories.CName == C && TT.Price > 9.99m && TT.Price < 20.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    //SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 3)
                {
                    var TTemp = from TT in db.Toy
                                where TT.Categories.CName == C && TT.Price > 19.99m && TT.Price < 50.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    //SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 4)
                {
                    var TTemp = from TT in db.Toy
                                where TT.Categories.CName == C && TT.Price > 49.99m && TT.Price < 100.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    //SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 5)
                {
                    var TTemp = from TT in db.Toy
                                where TT.Categories.CName == C && TT.Price > 99.99m && TT.Price < 500.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    //SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 6)
                {
                    var TTemp = from TT in db.Toy
                                where TT.Categories.CName == C && TT.Price > 500.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    //SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = OC.SubCategories,
                        PC = null
                    };
                    return View(a);
                }
                else
                {
                    return View();
                }
            } //X


            else if (C == null && SC == null && PC != null && search == null) // price page naar category filter
            {
                //ICollection<Toy> toy = null;

                if (PC == 1)
                {
                    var TTemp = from TT in db.Toy
                                where  TT.Price > 0.00m && TT.Price < 10.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    
                    //Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    //SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = null,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 2)
                {
                    var TTemp = from TT in db.Toy
                                where TT.Price > 9.99m && TT.Price < 20.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    //Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    //SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = null,
                        PC = null
                    };
                    return View(a);


                    //var TTemp = from TT in db.Toy
                    //            where TT.Categories.CName == C && TT.Price > 9.99m && TT.Price < 20.00m
                    //            select TT;
                    //var TToy = new List<Toy>();
                    //foreach (Toy t in TTemp)
                    //{
                    //    TToy.Add(t);
                    //}


                    ////var TToy = TTemp.First();
                    ////Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    ////SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    //BrowsePartial a = new BrowsePartial
                    //{
                    //    C = CL,
                    //    T = TToy,
                    //    SC = null,
                    //    PC = null
                    //};
                    //return View(a);
                }
                else if (PC == 3)
                {
                    var TTemp = from TT in db.Toy
                                where TT.Price > 19.99m && TT.Price < 50.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    //Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    //SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = null,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 4)
                {
                    var TTemp = from TT in db.Toy
                                where TT.Price > 49.99m && TT.Price < 100.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    //Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    //SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = null,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 5)
                {
                    var TTemp = from TT in db.Toy
                                where TT.Price > 99.99m && TT.Price < 500.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    //Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    //SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = null,
                        PC = null
                    };
                    return View(a);
                }
                else if (PC == 6)
                {
                    var TTemp = from TT in db.Toy
                                where TT.Price > 500.00m
                                select TT;
                    var TToy = new List<Toy>();
                    foreach (Toy t in TTemp)
                    {
                        TToy.Add(t);
                    }


                    //var TToy = TTemp.First();
                    //Categories OC = db.Categories.Include("Toy").Single(c => c.CName == C); //One Category
                    //SubCategories OSC = db.SubCategories.Include("Toy").Single(sc => sc.SCName == SC); //One SubCategory
                    BrowsePartial a = new BrowsePartial
                    {
                        C = CL,
                        T = TToy.ToPagedList(P ?? 1, PS),
                        SC = null,
                        PC = null
                    };
                    return View(a);
                }
                else
                {
                    return View();
                }
            } //X

            else if (C == null && SC == null && PC == null && search != null) //start page
            {
                List<Toy> Toys = db.Toy.ToList();

                var TST = db.Toy.Where(x => x.ToysName.Contains(search)).ToList();

                BrowsePartial a = new BrowsePartial
                {
                    C = CL,
                    T = TST.ToPagedList(P ?? 1, PS),
                    SC = null,
                    PC = null,
                };
                return View(a);
            }

            else //start page
            {
                List<Toy> Toys = db.Toy.ToList();
                BrowsePartial a = new BrowsePartial
                {
                    C = CL,
                    T = Toys.ToPagedList(P ?? 1, PS),
                    SC = null,
                    PC = null,
                };
                return View(a);
            }           
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

        public ActionResult IndexR(string search, int? P, int PS)
        {
            List<Categories> CL = db.Categories.ToList();
            List<Toy> Toys = db.Toy.ToList();
            if (search != "" | search != null)
            {
                Toys = db.Toy.Where(x => x.ToysName.Contains(search)).ToList();
            }
            BrowsePartial a = new BrowsePartial
            {
                C = CL,
                T = Toys.ToPagedList(P ?? 1, PS),
                SC = null,
                PC = null,
            };
            return View(a);
        }
    }
}