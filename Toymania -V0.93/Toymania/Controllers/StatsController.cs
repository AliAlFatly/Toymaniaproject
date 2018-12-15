using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toymania.Models;
using Toymania.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Toymania.Controllers;
using System.Web.Mvc.Ajax;
//using System.Web.Helpers.Chart;
using System.Web.Helpers;
using System.Globalization;
using Google.Apis;
using System.Threading;
using System.Data.Entity.SqlServer;

namespace Toymania.Controllers
{
    public class StatsController : Controller
    {
        // GET: Stats
        //functie schrijven die eerste datum en laatste neemt en dan vervolgens eerste jaar tot aan laatste returnt?
        //class schrijven die een lijst return met waardes zoals jaar/week/dag informatie aan de hand van de gegeven parameter?
        //groupby convert to string maand/week??

        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }


        /*-------------------------------------------------------------------------------------------------*/

        public class CT
        {
            public string SC { get; set; } //SubCategory
            public string C { get; set; } //Category
            public string Title { get; set; }
            public int? T { get; set; } //totaal verkocht
            public decimal? t { get; set; } //totaal winst of omzet(prijs)
        }

        public class CL
        {
            public int W { get; set; }
            public List<CT> ct { get; set; }
        }

        public class CLY
        {
            public int Y { get; set; }
            public List<CT> ct { get; set; }
        }

        public class PCLY
        {
            public int Y { get; set; }
            public CT ct { get; set; }
        }

        public class CLM
        {
            public int M { get; set; }
            public List<CT> ct { get; set; }
        }

        public class PCLM
        {
            public int M { get; set; }
            public CT ct { get; set; }
        }
        
        public class PCL
        {
            public int W { get; set; }
            public CT ct { get; set; }
        }

        public class CLD
        {
            public int D { get; set; }
            public List<CT> ct { get; set; }
        }


        /*-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public int TCY(int Y, string CName) //get total category(sales)
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.CName == CName
                     select (int)od.Quantity).ToList();

            var R = 0;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? PCY(int Y, string CName) //get total winst category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                    where od.year == Y && od.CName == CName
                    select od.Quantity * od.UnitPrice).ToList();
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I * 1.08m;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? RCY(int Y, string CName) //get total omzet category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = from od in db.OrderDetails
                    where od.year == Y && od.CName == CName
                    select od;
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            if (T != null)
            {
                decimal? R = 0;
                foreach (var i in T)
                {
                    R += (i.UnitPrice * i.Quantity);
                }
                return R;


            }
            else
            {
                decimal? R = 0;
                return R;
            }
        }



        public JsonResult YCS()
        {
            var sdy = (from o in db.Order
                      group o by o.OrderDate.Value.Year into b
                      select b.Key).ToList();

            var FY = sdy.FirstOrDefault();
            var LY = sdy.LastOrDefault();
            var YC = sdy.Count();

            //var t1 = (from o in db.OrderDetails
            //          group o by o.year into y
            //          select y).ToList();

            var t2 = (from o in db.OrderDetails
                      select o).ToList();

            var ODCL = new List<CLY>();
            if (t2 != null)
            {
                for (int? i = FY; i <= LY; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t2.Where(x => x.year == i) != null)
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                T = TCY((int)i, c.CName)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLY
                        {
                            Y = (int)i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                T = 0
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLY
                        {
                            Y = (int)i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[YC + 1];
                t6[0] = new object[]
                    {
                        "Year",
                        "Bord en Kaartspellen",
                        "Poppen",
                        "Speelvoertuigen",
                        "Video games",
                        "Puzzels",
                        "Lego",
                        "Bouwen",
                        "Boeken",
                        "Hobby en creatief",
                        "Baby en peuter",
                        "Buitenspeelgoed"
                    };

                var j = 0;
                foreach (var i in ODCL) { 
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] { i.Y.ToString(),
                            i.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Poppen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Video games").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Puzzels").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bouwen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Boeken").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Baby en peuter").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.T).FirstOrDefault(),


                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.Y.ToString(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult YCP()
        {
            var sdy = (from o in db.Order
                       group o by o.OrderDate.Value.Year into b
                       select b.Key).ToList();

            var FY = sdy.FirstOrDefault();
            var LY = sdy.LastOrDefault();
            var YC = sdy.Count();

            //var t1 = (from o in db.OrderDetails
            //          group o by o.year into y
            //          select y).ToList();

            var t2 = (from o in db.OrderDetails
                      select o).ToList();

            var ODCL = new List<CLY>();
            if (t2 != null)
            {
                for (int? i = FY; i <= LY; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t2.Where(x => x.year == i) != null)
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = PCY((int)i, c.CName)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLY
                        {
                            Y = (int)i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = 0
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLY
                        {
                            Y = (int)i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[YC + 1];
                t6[0] = new object[]
                    {
                        "Year",
                        "Bord en Kaartspellen",
                        "Poppen",
                        "Speelvoertuigen",
                        "Video games",
                        "Puzzels",
                        "Lego",
                        "Bouwen",
                        "Boeken",
                        "Hobby en creatief",
                        "Baby en peuter",
                        "Buitenspeelgoed"
                    };

                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] { i.Y.ToString(),
                            i.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Poppen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Video games").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Puzzels").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bouwen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Boeken").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Baby en peuter").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.t).FirstOrDefault(),


                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.Y.ToString(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult YCR()
        {
            var sdy = (from o in db.Order
                       group o by o.OrderDate.Value.Year into b
                       select b.Key).ToList();

            var FY = sdy.FirstOrDefault();
            var LY = sdy.LastOrDefault();
            var YC = sdy.Count();

            //var t1 = (from o in db.OrderDetails
            //          group o by o.year into y
            //          select y).ToList();

            var t2 = (from o in db.OrderDetails
                      select o).ToList();

            var ODCL = new List<CLY>();
            if (t2 != null)
            {
                for (int? i = FY; i <= LY; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t2.Where(x => x.year == i) != null)
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = RCY((int)i, c.CName)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLY
                        {
                            Y = (int)i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = 0
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLY
                        {
                            Y = (int)i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[YC + 1];
                t6[0] = new object[]
                    {
                        "Year",
                        "Bord en Kaartspellen",
                        "Poppen",
                        "Speelvoertuigen",
                        "Video games",
                        "Puzzels",
                        "Lego",
                        "Bouwen",
                        "Boeken",
                        "Hobby en creatief",
                        "Baby en peuter",
                        "Buitenspeelgoed"
                    };

                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] { i.Y.ToString(),
                            i.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Poppen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Video games").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Puzzels").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bouwen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Boeken").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Baby en peuter").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.t).FirstOrDefault(),


                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.Y.ToString(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        /*-------------------------------------------------------------------------------------------------*/
        public int TCM(int Y, string CName, int M) //get total category(sales)
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.CName == CName && od.Month == M
                     select (int)od.Quantity).ToList();

            var R = 0;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? WCM(int Y, string CName, int M) //get total winst category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                    where od.year == Y && od.CName == CName && od.Month == M
                    select od.Quantity * od.UnitPrice).ToList();
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I * 1.08m;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? OCM(int Y, string CName, int M) //get total omzet category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                    where od.year == Y && od.CName == CName && od.Month == M
                    select od.Quantity * od.UnitPrice).ToList();
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public JsonResult MCS(int y)
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<CLM>();
            if (t1 != null)
            {
                for (int i = 0; i <= 12; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Month == i) != null)
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                T = TCM(y, c.CName, i)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLM
                        {
                            M = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                T = 0
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLM
                        {
                            M = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }


            if (ODCL != null)
            {
                var t6 = new object[12 + 1];

                t6[0] = new object[]
                  {
                    "Month",
                    "Bord en Kaartspellen",
                    "Poppen",
                    "Speelvoertuigen",
                    "Video games",
                    "Puzzels",
                    "Lego",
                    "Bouwen",
                    "Boeken",
                    "Hobby en creatief",
                    "Baby en peuter",
                    "Buitenspeelgoed"
                  };
                for (int i = 1; i <= 12; i++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                    var MD = ODCL.Where(a => a.M.Equals(i)).FirstOrDefault();
                    if (MD != null)
                    {
                        t6[i] = new object[] { monthName,
                            MD.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Poppen").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Video games").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Puzzels").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Lego").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Bouwen").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Boeken").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Baby en peuter").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.T).FirstOrDefault(),


                        };
                    }
                    else
                    {
                        t6[i] = new object[] { monthName, 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult MCP(int y)
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<CLM>();
            if (t1 != null)
            {
                for (int i = 0; i <= 12; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Month == i) != null)
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = WCM(y, c.CName, i)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLM
                        {
                            M = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = 0m
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLM
                        {
                            M = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }


            if (ODCL != null)
            {
                var t6 = new object[12 + 1];

                t6[0] = new object[]
                  {
                    "Month",
                    "Bord en Kaartspellen",
                    "Poppen",
                    "Speelvoertuigen",
                    "Video games",
                    "Puzzels",
                    "Lego",
                    "Bouwen",
                    "Boeken",
                    "Hobby en creatief",
                    "Baby en peuter",
                    "Buitenspeelgoed"
                  };
                for (int i = 1; i <= 12; i++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                    var MD = ODCL.Where(a => a.M.Equals(i)).FirstOrDefault();
                    if (MD != null)
                    {
                        t6[i] = new object[] { monthName,
                            MD.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Poppen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Video games").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Puzzels").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Lego").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Bouwen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Boeken").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Baby en peuter").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.t).FirstOrDefault(),


                        };
                    }
                    else
                    {
                        t6[i] = new object[] { monthName, 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult MCR(int y)
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<CLM>();
            if (t1 != null)
            {
                for (int i = 0; i <= 12; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Month == i) != null)
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = OCM(y, c.CName, i)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLM
                        {
                            M = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = 0m
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLM
                        {
                            M = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }


            if (ODCL != null)
            {
                var t6 = new object[12 + 1];

                t6[0] = new object[]
                  {
                    "Month",
                    "Bord en Kaartspellen",
                    "Poppen",
                    "Speelvoertuigen",
                    "Video games",
                    "Puzzels",
                    "Lego",
                    "Bouwen",
                    "Boeken",
                    "Hobby en creatief",
                    "Baby en peuter",
                    "Buitenspeelgoed"
                  };
                for (int i = 1; i <= 12; i++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                    var MD = ODCL.Where(a => a.M.Equals(i)).FirstOrDefault();
                    if (MD != null)
                    {
                        t6[i] = new object[] { monthName,
                            MD.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Poppen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Video games").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Puzzels").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Lego").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Bouwen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Boeken").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Baby en peuter").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.t).FirstOrDefault(),


                        };
                    }
                    else
                    {
                        t6[i] = new object[] { monthName, 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }



        /*-------------------------------------------------------------------------------------------------*/
        public int GTC(int W, string CName, int y) //get total category(sales)
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                     where od.Week == W && od.CName == CName && od.year == y
                     select (int)od.Quantity).ToList();
            var R = 0;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? GTWC(int W, string CName, int y) //get total winst category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                    where od.Week == W && od.CName == CName && od.year == y
                    select od.Quantity * (decimal?)od.UnitPrice).ToList();
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I * 1.08m;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? GTOC(int W, string CName, int y) //get total omzet category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                    where od.Week == W && od.CName == CName && od.year == y
                    select od.Quantity * od.UnitPrice).ToList();
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }


          

        public JsonResult WCS(int y) //DATA WEEK CATEGORIES
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<CL>();
            if(t1 != null)
            {
                for(int i = 0; i <= 53; i++)
                {
                    var TEMPCT = new List<CT>();

                    if(t1.Where(x=> x.Week == i) != null)
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                T = GTC(i, c.CName, y)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CL
                        {
                            W = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach(var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                T = 0
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CL
                        {
                            W = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var wc = 53;
                var t6 = new object[wc + 1];
                t6[0] = new object[]
                    {
                        "Week",
                        "Bord en Kaartspellen",
                        "Poppen",
                        "Speelvoertuigen",
                        "Video games",
                        "Puzzels",
                        "Lego",
                        "Bouwen",
                        "Boeken",
                        "Hobby en creatief",
                        "Baby en peuter",
                        "Buitenspeelgoed"
                    };

                
                for (int j = 1; j <= wc; j++)
                {
                    var wD = ODCL.Where(c => c.W == j).FirstOrDefault();
                    if (wD != null)
                    {
                        t6[j] = new object[] { "week" + wD.W,
                            wD.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Poppen").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Video games").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Puzzels").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Lego").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Bouwen").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Boeken").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Baby en peuter").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.T).FirstOrDefault(),


                        };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult WCP(int y) //DATA WEEK CATEGORIES
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<CL>();
            if (t1 != null)
            {
                for (int i = 0; i <= 53; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Week == i) != null)
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = GTWC(i, c.CName, y) //deze functie is anders dan DWCVerkocht(enige wat verandert wordt/ winst ipv sum)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CL
                        {
                            W = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = 0m
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CL
                        {
                            W = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var wc = 53;
                var t6 = new object[wc + 1];
                t6[0] = new object[]
                    {
                        "Week",
                        "Bord en Kaartspellen",
                        "Poppen",
                        "Speelvoertuigen",
                        "Video games",
                        "Puzzels",
                        "Lego",
                        "Bouwen",
                        "Boeken",
                        "Hobby en creatief",
                        "Baby en peuter",
                        "Buitenspeelgoed"
                    };


                for (int j = 1; j <= wc; j++)
                {
                    var wD = ODCL.Where(c => c.W == j).FirstOrDefault();
                    if (wD != null)
                    {
                        t6[j] = new object[] { "week" + wD.W,
                            wD.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Poppen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Video games").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Puzzels").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Lego").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Bouwen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Boeken").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Baby en peuter").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.t).FirstOrDefault(),


                        };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult WCR(int y) //DATA WEEK CATEGORIES
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<CL>();
            if (t1 != null)
            {
                for (int i = 0; i <= 53; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Week == i) != null)
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = GTOC(i, c.CName, y) //deze functie is anders dan DWCVerkocht(enige wat verandert wordt/ omzet ipv sum)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CL
                        {
                            W = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = 0m
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CL
                        {
                            W = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var wc = 53;
                var t6 = new object[wc + 1];
                t6[0] = new object[]
                    {
                        "Week",
                        "Bord en Kaartspellen",
                        "Poppen",
                        "Speelvoertuigen",
                        "Video games",
                        "Puzzels",
                        "Lego",
                        "Bouwen",
                        "Boeken",
                        "Hobby en creatief",
                        "Baby en peuter",
                        "Buitenspeelgoed"
                    };


                for (int j = 1; j <= wc; j++)
                {
                    var wD = ODCL.Where(c => c.W == j).FirstOrDefault();
                    if (wD != null)
                    {
                        t6[j] = new object[] { "week" + wD.W,
                            wD.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Poppen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Video games").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Puzzels").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Lego").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Bouwen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Boeken").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Baby en peuter").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.t).FirstOrDefault(),


                        };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /*-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/        
        public int PYS(int Y) 
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y
                     select (int)od.Quantity).ToList();
            var R = 0;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? PYR(int Y)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y
                     select (int)od.Quantity * (decimal?)od.UnitPrice).ToList();
            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? PYP(int Y)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y
                     select (int)od.Quantity * (decimal?)od.UnitPrice).ToList();
            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I * 1.08m;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }



        public int PMSc(int Y, int M)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.Month == M
                     select (int)od.Quantity).ToList();
            var R = 0;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? PMRc(int Y, int M)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.Month == M
                     select (int)od.Quantity * (decimal?)od.UnitPrice).ToList();
            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? PMPc(int Y, int M)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.Month == M
                     select (int)od.Quantity * (decimal?)od.UnitPrice).ToList();
            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I * 1.08m;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }



        public int PWSc(int Y, int W)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.Week == W
                     select (int)od.Quantity).ToList();
            var R = 0;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? PWRc(int Y, int W)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.Week == W
                     select (int)od.Quantity * (decimal?)od.UnitPrice).ToList();
            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? PWPc(int Y, int W)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.Week == W
                     select (int)od.Quantity * (decimal?)od.UnitPrice).ToList();
            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I * 1.08m;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }



        public int PCSc(int Y, string CN)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.CName == CN
                     select (int)od.Quantity).ToList();
            var R = 0;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? PCRc(int Y, string CN)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.CName == CN
                     select (int)od.Quantity * (decimal?)od.UnitPrice).ToList();
            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? PCPc(int Y, string CN)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.CName == CN
                     select (int)od.Quantity * (decimal?)od.UnitPrice).ToList();
            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I * 1.08m;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }



        public int PSSc(int Y, string SCN)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.SCName == SCN
                     select (int)od.Quantity).ToList();
            var R = 0;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? PSRc(int Y, string SCN)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.SCName == SCN
                     select (int)od.Quantity * (decimal?)od.UnitPrice).ToList();
            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? PSPc(int Y, string SCN)
        {
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.SCName == SCN
                     select (int)od.Quantity * (decimal?)od.UnitPrice).ToList();
            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I * 1.08m;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        /*-------------------Year-average-------------------*/

        public JsonResult PYCS()
        {
            var sdy = (from o in db.Order
                       group o by o.OrderDate.Value.Year into b
                       select b.Key).ToList();

            //var PYD = (from o in db.Order
            //           group o by o.year into b
            //           select new
            //           {
            //               year = b.Key,
            //               sales = b.Select(x => x.Total).Count()
            //           }).ToList();

            var FY = sdy.FirstOrDefault();
            var LY = sdy.LastOrDefault();
            var YC = sdy.Count();

            //var t1 = (from o in db.OrderDetails
            //          group o by o.year into y
            //          select y).ToList();

            var t2 = (from o in db.OrderDetails
                      select o).ToList();

            var ODCL = new List<PCLY>();
            if (t2 != null)
            {
                for (int? i = FY; i <= LY; i++)
                {
                    if (t2.Where(x => x.year == i) != null)
                    {
                        var T2 = new CT
                        {
                            Title = "A",
                            T = PYS((int)i)
                        };
                        var TEMPL = new PCLY
                        {
                            Y = (int)i,
                            ct = T2
                        };

                        ODCL.Add(TEMPL);
                    }
                    else
                    {
                        //!!!!!!!!!!!!!!!!!!!!//
                    }
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[YC + 1];
                t6[0] = new object[]
                    {
                        "Year",
                        "Total sales",
                    };


                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] {
                            i.Y.ToString(),
                            i.ct.T
                            //i.ct.Where(x => x.C == "Average").Select(c => c.T).FirstOrDefault(),
                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.Y.ToString(), 0 };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult PYCP()
        {
            var sdy = (from o in db.Order
                       group o by o.OrderDate.Value.Year into b
                       select b.Key).ToList();

            //var PYD = (from o in db.Order
            //           group o by o.year into b
            //           select new
            //           {
            //               year = b.Key,
            //               sales = b.Select(x => x.Total).Count()
            //           }).ToList();

            var FY = sdy.FirstOrDefault();
            var LY = sdy.LastOrDefault();
            var YC = sdy.Count();

            //var t1 = (from o in db.OrderDetails
            //          group o by o.year into y
            //          select y).ToList();

            var t2 = (from o in db.OrderDetails
                      select o).ToList();

            var ODCL = new List<PCLY>();
            if (t2 != null)
            {
                for (int? i = FY; i <= LY; i++)
                {
                    if (t2.Where(x => x.year == i) != null)
                    {
                        var T2 = new CT
                        {
                            Title = "A",
                            t = PYP((int)i)
                        };
                        var TEMPL = new PCLY
                        {
                            Y = (int)i,
                            ct = T2
                        };

                        ODCL.Add(TEMPL);
                    }
                    else
                    {

                    }
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[YC + 1];
                t6[0] = new object[]
                    {
                        "Year",
                        "Total profit",
                    };


                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] {
                            i.Y.ToString(),
                            i.ct.t
                            //i.ct.Where(x => x.C == "Average").Select(c => c.T).FirstOrDefault(),
                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.Y.ToString(), 0 };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult PYCR()
        {
            var sdy = (from o in db.Order
                       group o by o.OrderDate.Value.Year into b
                       select b.Key).ToList();

            //var PYD = (from o in db.Order
            //           group o by o.year into b
            //           select new
            //           {
            //               year = b.Key,
            //               sales = b.Select(x => x.Total).Count()
            //           }).ToList();

            var FY = sdy.FirstOrDefault();
            var LY = sdy.LastOrDefault();
            var YC = sdy.Count();

            //var t1 = (from o in db.OrderDetails
            //          group o by o.year into y
            //          select y).ToList();

            var t2 = (from o in db.OrderDetails
                      select o).ToList();

            var ODCL = new List<PCLY>();
            if (t2 != null)
            {
                for (int? i = FY; i <= LY; i++)
                {
                    if (t2.Where(x => x.year == i) != null)
                    {
                        var T2 = new CT
                        {
                            Title = "A",
                            t = PYR((int)i)
                        };
                        var TEMPL = new PCLY
                        {
                            Y = (int)i,
                            ct = T2
                        };

                        ODCL.Add(TEMPL);
                    }
                    else
                    {

                    }
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[YC + 1];
                t6[0] = new object[]
                    {
                        "Year",
                        "Total revenue",
                    };


                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] {
                            i.Y.ToString(),
                            i.ct.t
                            //i.ct.Where(x => x.C == "Average").Select(c => c.T).FirstOrDefault(),
                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.Y.ToString(), 0 };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }



        /*---------------------Monthly----------------------*/

        public JsonResult PMS(int y)
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<PCLM>();
            if (t1 != null)
            {
                for (int i = 0; i <= 12; i++)
                {

                    var T2 = new CT
                    {
                        Title = "A",
                        T = PMSc((int)y, i)
                    };
                    var TEMPL = new PCLM
                    {
                        M = (int)i,
                        ct = T2
                    };

                    ODCL.Add(TEMPL);
                }
            }


            if (ODCL != null) //anders category - totaal en foreach category totaal toevoegen
            {
                var t6 = new object[12 + 1];

                t6[0] = new object[]
                  {
                    "Month",
                    "Total Sales Month",

                  };
                for (int i = 1; i <= 12; i++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                    var MD = ODCL.Where(a => a.M.Equals(i)).FirstOrDefault();
                    if (MD != null)
                    {
                        t6[i] = new object[] { monthName,
                            MD.ct.T

                        };
                    }
                    else
                    {
                        t6[i] = new object[] { monthName, 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult PMP(int y)
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<PCLM>();
            if (t1 != null)
            {
                for (int i = 0; i <= 12; i++)
                {

                    var T2 = new CT
                    {
                        Title = "A",
                        t = PMPc((int)y, i)
                    };
                    var TEMPL = new PCLM
                    {
                        M = (int)i,
                        ct = T2
                    };

                    ODCL.Add(TEMPL);
                }
            }


            if (ODCL != null) //anders category - totaal en foreach category totaal toevoegen
            {
                var t6 = new object[12 + 1];

                t6[0] = new object[]
                  {
                    "Month",
                    "Total Sales Month",

                  };
                for (int i = 1; i <= 12; i++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                    var MD = ODCL.Where(a => a.M.Equals(i)).FirstOrDefault();
                    if (MD != null)
                    {
                        t6[i] = new object[] { monthName,
                            MD.ct.t

                        };
                    }
                    else
                    {
                        t6[i] = new object[] { monthName, 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult PMR(int y)
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<PCLM>();
            if (t1 != null)
            {
                for (int i = 0; i <= 12; i++)
                {

                    var T2 = new CT
                    {
                        Title = "A",
                        t = PMRc((int)y, i)
                    };
                    var TEMPL = new PCLM
                    {
                        M = (int)i,
                        ct = T2
                    };

                    ODCL.Add(TEMPL);
                }
            }


            if (ODCL != null) //anders category - totaal en foreach category totaal toevoegen
            {
                var t6 = new object[12 + 1];

                t6[0] = new object[]
                  {
                    "Month",
                    "Total Sales Month",

                  };
                for (int i = 1; i <= 12; i++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                    var MD = ODCL.Where(a => a.M.Equals(i)).FirstOrDefault();
                    if (MD != null)
                    {
                        t6[i] = new object[] { monthName,
                            MD.ct.t

                        };
                    }
                    else
                    {
                        t6[i] = new object[] { monthName, 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        /*---------------------Weekly----------------------*/


        public JsonResult PWS(int y)
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<PCL>();
            if (t1 != null)
            {
                for (int i = 0; i <= 53; i++)
                {

                    var T2 = new CT
                    {
                        Title = "A",
                        T = PWSc((int)y, i)
                    };
                    var TEMPL = new PCL
                    {
                        W = (int)i,
                        ct = T2
                    };

                    ODCL.Add(TEMPL);
                }
            }


            if (ODCL != null) //anders category - totaal en foreach category totaal toevoegen
            {
                var t6 = new object[53 + 1];

                t6[0] = new object[]
                  {
                    "Week",
                    "Total Sales Month",

                  };
                for (int i = 1; i <= 53; i++)
                {
                    var MD = ODCL.Where(w => w.W == i).SingleOrDefault();
                    if (MD != null)
                    {
                        t6[i] = new object[] { "week" + MD.W,
                            MD.ct.T
                        };
                    }
                    else
                    {
                        t6[i] = new object[] { "week" + i, 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult PWP(int y)
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<PCL>();
            if (t1 != null)
            {
                for (int i = 0; i <= 53; i++)
                {

                    var T2 = new CT
                    {
                        Title = "A",
                        t = PWPc((int)y, i)
                    };
                    var TEMPL = new PCL
                    {
                        W = (int)i,
                        ct = T2
                    };

                    ODCL.Add(TEMPL);
                }
            }


            if (ODCL != null) //anders category - totaal en foreach category totaal toevoegen
            {
                var t6 = new object[53 + 1];

                t6[0] = new object[]
                  {
                    "Week",
                    "Total Sales Month",

                  };
                for (int i = 1; i <= 53; i++)
                {
                    var MD = ODCL.Where(w => w.W == i).SingleOrDefault();
                    if (MD != null)
                    {
                        t6[i] = new object[] { "week" + MD.W,
                            MD.ct.t
                        };
                    }
                    else
                    {
                        t6[i] = new object[] { "week" + i, 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult PWR(int y)
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<PCL>();
            if (t1 != null)
            {
                for (int i = 0; i <= 53; i++)
                {

                    var T2 = new CT
                    {
                        Title = "A",
                        t = PWRc((int)y, i)
                    };
                    var TEMPL = new PCL
                    {
                        W = (int)i,
                        ct = T2
                    };

                    ODCL.Add(TEMPL);
                }
            }


            if (ODCL != null) //anders category - totaal en foreach category totaal toevoegen
            {
                var t6 = new object[53 + 1];

                t6[0] = new object[]
                  {
                    "Week",
                    "Total Sales Month",

                  };
                for (int i = 1; i <= 53; i++)
                {
                    var MD = ODCL.Where(w => w.W == i).SingleOrDefault();
                    if (MD != null)
                    {
                        t6[i] = new object[] { "week" + MD.W,
                            MD.ct.t
                        };
                    }
                    else
                    {
                        t6[i] = new object[] { "week" + i, 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }



        /*---------------Categorybased-yearly---------------*/

        public JsonResult PCS(int Y)
        {
            var t2 = (from o in db.OrderDetails
                      where o.year == Y
                      select o).ToList();
            var CLC = 0;
            var ODCL = new List<CT>();
            if (t2 != null)
            {
                var CC = from C in db.Categories
                         select C;
                CLC = CC.Count();
                foreach (var c in CC)
                {
                    var T2 = new CT
                    {
                        C = c.CName,
                        T = PCSc(Y, c.CName)
                    };
                    ODCL.Add(T2);
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[CLC + 1];
                t6[0] = new object[]
                    {
                        "Category",
                        "Total Sales",
                    };

                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] { i.C,
                            i.T
                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.C, 0 };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult PCR(int Y)
        {
            var t2 = (from o in db.OrderDetails
                      where o.year == Y
                      select o).ToList();
            var CLC = 0;
            var ODCL = new List<CT>();
            if (t2 != null)
            {
                var CC = from C in db.Categories
                         select C;
                CLC = CC.Count();
                foreach (var c in CC)
                {
                    var T2 = new CT
                    {
                        C = c.CName,
                        t = PCRc(Y, c.CName)
                    };
                    ODCL.Add(T2);
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[CLC + 1];
                t6[0] = new object[]
                    {
                        "Category",
                        "Total Sales",
                    };

                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] { i.C,
                            i.t
                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.C, 0 };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult PCP(int Y)
        {
            var t2 = (from o in db.OrderDetails
                      where o.year == Y
                      select o).ToList();
            var CLC = 0;
            var ODCL = new List<CT>();
            if (t2 != null)
            {
                var CC = from C in db.Categories
                         select C;
                CLC = CC.Count();
                foreach (var c in CC)
                {
                    var T2 = new CT
                    {
                        C = c.CName,
                        t = PCPc(Y, c.CName)
                    };
                    ODCL.Add(T2);
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[CLC + 1];
                t6[0] = new object[]
                    {
                        "Category",
                        "Total Sales",
                    };

                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] { i.C,
                            i.t
                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.C, 0 };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /*-------------SubCategorybased-yearly--------------*/

        public JsonResult PSS(int Y)
        {
            var t2 = (from o in db.OrderDetails
                      where o.year == Y
                      select o).ToList();
            var CLC = 0;
            var ODCL = new List<CT>();
            if (t2 != null)
            {
                var CC = from C in db.SubCategories
                         select C;
                CLC = CC.Count();
                foreach (var c in CC)
                {
                    var T2 = new CT
                    {
                        C = c.SCName,
                        T = PSSc(Y, c.SCName)
                    };
                    ODCL.Add(T2);
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[CLC + 1];
                t6[0] = new object[]
                    {
                        "Subcategories",
                        "Total Sales",
                    };

                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] { i.C,
                            i.T
                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.C, 0 };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult PSR(int Y)
        {
            var t2 = (from o in db.OrderDetails
                      where o.year == Y
                      select o).ToList();
            var CLC = 0;
            var ODCL = new List<CT>();
            if (t2 != null)
            {
                var CC = from C in db.SubCategories
                         select C;
                CLC = CC.Count();
                foreach (var c in CC)
                {
                    var T2 = new CT
                    {
                        C = c.SCName,
                        t = PSRc(Y, c.SCName)
                    };
                    ODCL.Add(T2);
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[CLC + 1];
                t6[0] = new object[]
                    {
                        "Subcategories",
                        "Total Sales",
                    };

                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] { i.C,
                            i.t
                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.C, 0 };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult PSP(int Y)
        {
            var t2 = (from o in db.OrderDetails
                      where o.year == Y
                      select o).ToList();
            var CLC = 0;
            var ODCL = new List<CT>();
            if (t2 != null)
            {
                var CC = from C in db.SubCategories
                         select C;
                CLC = CC.Count();
                foreach (var c in CC)
                {
                    var T2 = new CT
                    {
                        C = c.SCName,
                        t = PSPc(Y, c.SCName)
                    };
                    ODCL.Add(T2);
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[CLC + 1];
                t6[0] = new object[]
                    {
                        "Subcategory",
                        "Total Sales",
                    };

                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] { i.C,
                            i.t
                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.C, 0 };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }




        /*----------------------------------------------------------------------------------------------------SC----------------------------------------------------------------------------------------------------*/

        public int TSY(int Y, string SCName) //get total category(sales)
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.SCName == SCName
                     select (int)od.Quantity).ToList();

            var R = 0;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? PSY(int Y, string SCName) //get total winst category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.SCName == SCName
                     select od.Quantity * od.UnitPrice).ToList();
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I * 1.08m;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? RSY(int Y, string SCName) //get total omzet category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = from od in db.OrderDetails
                    where od.year == Y && od.SCName == SCName
                    select od;
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            if (T != null)
            {
                decimal? R = 0;
                foreach (var i in T)
                {
                    R += (i.UnitPrice * i.Quantity);
                }
                return R;


            }
            else
            {
                decimal? R = 0;
                return R;
            }
        }



        public JsonResult YSS()
        {
            var sdy = (from o in db.Order
                       group o by o.OrderDate.Value.Year into b
                       select b.Key).ToList();

            var FY = sdy.FirstOrDefault();
            var LY = sdy.LastOrDefault();
            var YC = sdy.Count();

            //var t1 = (from o in db.OrderDetails
            //          group o by o.year into y
            //          select y).ToList();

            var t2 = (from o in db.OrderDetails
                      select o).ToList();

            var ODCL = new List<CLY>();
            if (t2 != null)
            {
                for (int? i = FY; i <= LY; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t2.Where(x => x.year == i) != null)
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                T = TSY((int)i, c.SCName)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLY
                        {
                            Y = (int)i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                T = 0
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLY
                        {
                            Y = (int)i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[YC + 1];
                t6[0] = new object[]
                    {
                        "Year",
                        "Xbox1",
                        "Switch",
                        "PS4",
                        "Bordspellen",
                        "Kaartspellen",
                        "Partyspellen",
                        "Kinderpuzzels",
                        "3D puzzels",
                        "Barbie",
                        "Baby-born",
                        "Poppenhuis",
                        "Speelauto's en racebanen",
                        "Garages",
                        "Treinen en treinbanen",
                        "Drones",
                        "Kleurboeken",
                        "Prentenboeken",
                        "Fictie boeken",
                        "Bijtringen en rammelaars",
                        "Badspeelgoed",
                        "Stapelspeelgoed",
                        "Vormpjesspeelgoed",
                        "Overig babyspeelgoed",
                        "Lego city",
                        "Lego Ninjago",
                        "Lego Duplo",
                        "K'nex",
                        "BRIO",
                        "Speeltoestellen",
                        "Zandspeelgoed",
                        "Zwembaden",
                        "Rijden speelgoed",
                        "Knutselpakketten",
                        "Tekenen",
                        "Schilderen"
                    };

                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] { i.Y.ToString(),
                            i.ct.Where(x => x.C == "Xbox1").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Switch").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "PS4").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bordspellen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Kaartspellen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Partyspellen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Kinderpuzzels").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "3D puzzels").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Barbie").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Baby-born").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Poppenhuis").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speelauto's en racebanen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Garages").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Treinen en treinbanen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Drones").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Kleurboeken").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Prentenboeken").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Fictie boeken").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bijtringen en rammelaars").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Badspeelgoed").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Stapelspeelgoed").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Vormpjesspeelgoed").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Overig babyspeelgoed").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego city").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego Ninjago").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego Duplo").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "K'nex").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "BRIO").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speeltoestellen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Zandspeelgoed").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Zwembaden").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Rijden speelgoed").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Knutselpakketten").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Tekenen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Schilderen").Select(c => c.T).FirstOrDefault()
                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.Y.ToString(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult YSP()
        {
            var sdy = (from o in db.Order
                       group o by o.OrderDate.Value.Year into b
                       select b.Key).ToList();

            var FY = sdy.FirstOrDefault();
            var LY = sdy.LastOrDefault();
            var YC = sdy.Count();

            //var t1 = (from o in db.OrderDetails
            //          group o by o.year into y
            //          select y).ToList();

            var t2 = (from o in db.OrderDetails
                      select o).ToList();

            var ODCL = new List<CLY>();
            if (t2 != null)
            {
                for (int? i = FY; i <= LY; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t2.Where(x => x.year == i) != null)
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                t = PSY((int)i, c.SCName)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLY
                        {
                            Y = (int)i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                t = 0
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLY
                        {
                            Y = (int)i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[YC + 1];
                t6[0] = new object[]
                    {
                        "Year",
                        "Xbox1",
                        "Switch",
                        "PS4",
                        "Bordspellen",
                        "Kaartspellen",
                        "Partyspellen",
                        "Kinderpuzzels",
                        "3D puzzels",
                        "Barbie",
                        "Baby-born",
                        "Poppenhuis",
                        "Speelauto's en racebanen",
                        "Garages",
                        "Treinen en treinbanen",
                        "Drones",
                        "Kleurboeken",
                        "Prentenboeken",
                        "Fictie boeken",
                        "Bijtringen en rammelaars",
                        "Badspeelgoed",
                        "Stapelspeelgoed",
                        "Vormpjesspeelgoed",
                        "Overig babyspeelgoed",
                        "Lego city",
                        "Lego Ninjago",
                        "Lego Duplo",
                        "K'nex",
                        "BRIO",
                        "Speeltoestellen",
                        "Zandspeelgoed",
                        "Zwembaden",
                        "Rijden speelgoed",
                        "Knutselpakketten",
                        "Tekenen",
                        "Schilderen"
                    };

                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] { i.Y.ToString(),
                            i.ct.Where(x => x.C == "Xbox1").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Switch").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "PS4").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bordspellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Partyspellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Kinderpuzzels").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "3D puzzels").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Barbie").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Baby-born").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Poppenhuis").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speelauto's en racebanen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Garages").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Treinen en treinbanen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Drones").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Kleurboeken").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Prentenboeken").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Fictie boeken").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bijtringen en rammelaars").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Badspeelgoed").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Stapelspeelgoed").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Vormpjesspeelgoed").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Overig babyspeelgoed").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego city").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego Ninjago").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego Duplo").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "K'nex").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "BRIO").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speeltoestellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Zandspeelgoed").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Zwembaden").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Rijden speelgoed").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Knutselpakketten").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Tekenen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Schilderen").Select(c => c.t).FirstOrDefault()
                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.Y.ToString(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult YSR()
        {
            var sdy = (from o in db.Order
                       group o by o.year into b
                       select b.Key).ToList();

            var FY = sdy.FirstOrDefault();
            var LY = sdy.LastOrDefault();
            var YC = sdy.Count();

            //var t1 = (from o in db.OrderDetails
            //          group o by o.year into y
            //          select y).ToList();

            var t2 = (from o in db.OrderDetails
                      select o).ToList();

            var ODCL = new List<CLY>();
            if (t2 != null)
            {
                for (int? i = FY; i <= LY; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t2.Where(x => x.year == i) != null)
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                t = RSY((int)i, c.SCName)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLY
                        {
                            Y = (int)i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                t = 0
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLY
                        {
                            Y = (int)i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[YC + 1];
                t6[0] = new object[]
                    {
                        "Year",
                        "Xbox1",
                        "Switch",
                        "PS4",
                        "Bordspellen",
                        "Kaartspellen",
                        "Partyspellen",
                        "Kinderpuzzels",
                        "3D puzzels",
                        "Barbie",
                        "Baby-born",
                        "Poppenhuis",
                        "Speelauto's en racebanen",
                        "Garages",
                        "Treinen en treinbanen",
                        "Drones",
                        "Kleurboeken",
                        "Prentenboeken",
                        "Fictie boeken",
                        "Bijtringen en rammelaars",
                        "Badspeelgoed",
                        "Stapelspeelgoed",
                        "Vormpjesspeelgoed",
                        "Overig babyspeelgoed",
                        "Lego city",
                        "Lego Ninjago",
                        "Lego Duplo",
                        "K'nex",
                        "BRIO",
                        "Speeltoestellen",
                        "Zandspeelgoed",
                        "Zwembaden",
                        "Rijden speelgoed",
                        "Knutselpakketten",
                        "Tekenen",
                        "Schilderen"
                    };

                var j = 0;
                foreach (var i in ODCL)
                {
                    j++;
                    if (i != null)
                    {
                        t6[j] = new object[] { i.Y.ToString(),
                            i.ct.Where(x => x.C == "Xbox1").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Switch").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "PS4").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bordspellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Partyspellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Kinderpuzzels").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "3D puzzels").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Barbie").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Baby-born").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Poppenhuis").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speelauto's en racebanen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Garages").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Treinen en treinbanen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Drones").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Kleurboeken").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Prentenboeken").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Fictie boeken").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bijtringen en rammelaars").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Badspeelgoed").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Stapelspeelgoed").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Vormpjesspeelgoed").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Overig babyspeelgoed").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego city").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego Ninjago").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego Duplo").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "K'nex").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "BRIO").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speeltoestellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Zandspeelgoed").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Zwembaden").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Rijden speelgoed").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Knutselpakketten").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Tekenen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Schilderen").Select(c => c.t).FirstOrDefault()
                        };
                    }
                    else
                    {
                        t6[j] = new object[] { i.Y.ToString(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        /*-------------------------------------------------------------------------------------------------*/
        public int TSM(int Y, string SCName, int M) //get total category(sales)
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.SCName == SCName && od.Month == M
                     select (int)od.Quantity).ToList();

            var R = 0;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? WSM(int Y, string SCName, int M) //get total winst category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.SCName == SCName && od.Month == M
                     select od.Quantity * od.UnitPrice).ToList();
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I * 1.08m;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? OSM(int Y, string SCName, int M) //get total omzet category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.SCName == SCName && od.Month == M
                     select od.Quantity * od.UnitPrice).ToList();
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public JsonResult MSS(int y)
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<CLM>();
            if (t1 != null)
            {
                for (int i = 0; i <= 12; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Month == i) != null)
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                T = TSM(y, c.SCName, i)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLM
                        {
                            M = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                T = 0
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLM
                        {
                            M = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }


            if (ODCL != null)
            {
                var t6 = new object[12 + 1];

                t6[0] = new object[]
                    {
                        "Month",
                        "Xbox1",
                        "Switch",
                        "PS4",
                        "Bordspellen",
                        "Kaartspellen",
                        "Partyspellen",
                        "Kinderpuzzels",
                        "3D puzzels",
                        "Barbie",
                        "Baby-born",
                        "Poppenhuis",
                        "Speelauto's en racebanen",
                        "Garages",
                        "Treinen en treinbanen",
                        "Drones",
                        "Kleurboeken",
                        "Prentenboeken",
                        "Fictie boeken",
                        "Bijtringen en rammelaars",
                        "Badspeelgoed",
                        "Stapelspeelgoed",
                        "Vormpjesspeelgoed",
                        "Overig babyspeelgoed",
                        "Lego city",
                        "Lego Ninjago",
                        "Lego Duplo",
                        "K'nex",
                        "BRIO",
                        "Speeltoestellen",
                        "Zandspeelgoed",
                        "Zwembaden",
                        "Rijden speelgoed",
                        "Knutselpakketten",
                        "Tekenen",
                        "Schilderen"
                    };


                for (int i = 1; i <= 12; i++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                    var MD = ODCL.Where(a => a.M.Equals(i)).FirstOrDefault();
                    if (MD != null)
                    {
                        t6[i] = new object[] { monthName,
                            MD.ct.Where(x => x.C == "Xbox1").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Switch").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "PS4").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Bordspellen").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Kaartspellen").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Partyspellen").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Kinderpuzzels").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "3D puzzels").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Barbie").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Baby-born").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Poppenhuis").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Speelauto's en racebanen").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Garages").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Treinen en treinbanen").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Drones").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Kleurboeken").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Prentenboeken").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Fictie boeken").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Bijtringen en rammelaars").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Badspeelgoed").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Stapelspeelgoed").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Vormpjesspeelgoed").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Overig babyspeelgoed").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Lego city").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Lego Ninjago").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Lego Duplo").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "K'nex").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "BRIO").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Speeltoestellen").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Zandspeelgoed").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Zwembaden").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Rijden speelgoed").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Knutselpakketten").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Tekenen").Select(c => c.T).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Schilderen").Select(c => c.T).FirstOrDefault()
                        };
                    }
                    else
                    {
                        t6[i] = new object[] { monthName, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult MSP(int y)
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<CLM>();
            if (t1 != null)
            {
                for (int i = 0; i <= 12; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Month == i) != null)
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                t = WSM(y, c.SCName, i)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLM
                        {
                            M = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                t = 0m
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLM
                        {
                            M = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }


            if (ODCL != null)
            {
                var t6 = new object[12 + 1];

                t6[0] = new object[]
                    {
                        "Month",
                        "Xbox1",
                        "Switch",
                        "PS4",
                        "Bordspellen",
                        "Kaartspellen",
                        "Partyspellen",
                        "Kinderpuzzels",
                        "3D puzzels",
                        "Barbie",
                        "Baby-born",
                        "Poppenhuis",
                        "Speelauto's en racebanen",
                        "Garages",
                        "Treinen en treinbanen",
                        "Drones",
                        "Kleurboeken",
                        "Prentenboeken",
                        "Fictie boeken",
                        "Bijtringen en rammelaars",
                        "Badspeelgoed",
                        "Stapelspeelgoed",
                        "Vormpjesspeelgoed",
                        "Overig babyspeelgoed",
                        "Lego city",
                        "Lego Ninjago",
                        "Lego Duplo",
                        "K'nex",
                        "BRIO",
                        "Speeltoestellen",
                        "Zandspeelgoed",
                        "Zwembaden",
                        "Rijden speelgoed",
                        "Knutselpakketten",
                        "Tekenen",
                        "Schilderen"
                    };


                for (int i = 1; i <= 12; i++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                    var MD = ODCL.Where(a => a.M.Equals(i)).FirstOrDefault();
                    if (MD != null)
                    {
                        t6[i] = new object[] { monthName,
                            MD.ct.Where(x => x.C == "Xbox1").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Switch").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "PS4").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Bordspellen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Partyspellen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Kinderpuzzels").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "3D puzzels").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Barbie").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Baby-born").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Poppenhuis").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Speelauto's en racebanen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Garages").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Treinen en treinbanen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Drones").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Kleurboeken").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Prentenboeken").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Fictie boeken").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Bijtringen en rammelaars").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Badspeelgoed").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Stapelspeelgoed").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Vormpjesspeelgoed").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Overig babyspeelgoed").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Lego city").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Lego Ninjago").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Lego Duplo").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "K'nex").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "BRIO").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Speeltoestellen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Zandspeelgoed").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Zwembaden").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Rijden speelgoed").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Knutselpakketten").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Tekenen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Schilderen").Select(c => c.t).FirstOrDefault()
                        };
                    }
                    else
                    {
                        t6[i] = new object[] { monthName, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult MSR(int y)
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<CLM>();
            if (t1 != null)
            {
                for (int i = 0; i <= 12; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Month == i) != null)
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                t = OSM(y, c.SCName, i)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLM
                        {
                            M = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                t = 0m
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLM
                        {
                            M = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }


            if (ODCL != null)
            {
                var t6 = new object[12 + 1];

                t6[0] = new object[]
                    {
                        "Month",
                        "Xbox1",
                        "Switch",
                        "PS4",
                        "Bordspellen",
                        "Kaartspellen",
                        "Partyspellen",
                        "Kinderpuzzels",
                        "3D puzzels",
                        "Barbie",
                        "Baby-born",
                        "Poppenhuis",
                        "Speelauto's en racebanen",
                        "Garages",
                        "Treinen en treinbanen",
                        "Drones",
                        "Kleurboeken",
                        "Prentenboeken",
                        "Fictie boeken",
                        "Bijtringen en rammelaars",
                        "Badspeelgoed",
                        "Stapelspeelgoed",
                        "Vormpjesspeelgoed",
                        "Overig babyspeelgoed",
                        "Lego city",
                        "Lego Ninjago",
                        "Lego Duplo",
                        "K'nex",
                        "BRIO",
                        "Speeltoestellen",
                        "Zandspeelgoed",
                        "Zwembaden",
                        "Rijden speelgoed",
                        "Knutselpakketten",
                        "Tekenen",
                        "Schilderen"
                    };


                for (int i = 1; i <= 12; i++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                    var MD = ODCL.Where(a => a.M.Equals(i)).FirstOrDefault();
                    if (MD != null)
                    {
                        t6[i] = new object[] { monthName,
                            MD.ct.Where(x => x.C == "Xbox1").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Switch").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "PS4").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Bordspellen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Partyspellen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Kinderpuzzels").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "3D puzzels").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Barbie").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Baby-born").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Poppenhuis").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Speelauto's en racebanen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Garages").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Treinen en treinbanen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Drones").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Kleurboeken").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Prentenboeken").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Fictie boeken").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Bijtringen en rammelaars").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Badspeelgoed").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Stapelspeelgoed").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Vormpjesspeelgoed").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Overig babyspeelgoed").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Lego city").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Lego Ninjago").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Lego Duplo").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "K'nex").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "BRIO").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Speeltoestellen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Zandspeelgoed").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Zwembaden").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Rijden speelgoed").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Knutselpakketten").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Tekenen").Select(c => c.t).FirstOrDefault(),
                            MD.ct.Where(x => x.C == "Schilderen").Select(c => c.t).FirstOrDefault()
                        };
                    }
                    else
                    {
                        t6[i] = new object[] { monthName, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }



        /*-------------------------------------------------------------------------------------------------*/
        public int GTS(int W, string SCName, int y) //get total category(sales)
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                     where od.Week == W && od.SCName == SCName && od.year == y
                     select (int)od.Quantity).ToList();
            var R = 0;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? GTWS(int W, string SCName, int y) //get total winst category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                     where od.Week == W && od.SCName == SCName && od.year == y
                     select od.Quantity * (decimal?)od.UnitPrice).ToList();
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I * 1.08m;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }

        public decimal? GTOS(int W, string SCName, int y) //get total omzet category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                     where od.Week == W && od.SCName == SCName && od.year == y
                     select od.Quantity * od.UnitPrice).ToList();
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            decimal? R = 0.00m;
            if (T != null)
            {
                foreach (var I in T)
                {
                    if (I != null)
                    {
                        R += I;
                    }
                    else
                    {
                        R += 0;
                    }
                }
                //var R = (int)T;
                return R;
            }
            else
            {
                return R;
            }
        }




        public JsonResult WSS(int y) //DATA WEEK CATEGORIES
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<CL>();
            if (t1 != null)
            {
                for (int i = 0; i <= 53; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Week == i) != null)
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                T = GTS(i, c.SCName, y)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CL
                        {
                            W = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                T = 0
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CL
                        {
                            W = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var wc = 53;
                var t6 = new object[wc + 1];
                t6[0] = new object[]
                    {
                        "Week",
                        "Xbox1",
                        "Switch",
                        "PS4",
                        "Bordspellen",
                        "Kaartspellen",
                        "Partyspellen",
                        "Kinderpuzzels",
                        "3D puzzels",
                        "Barbie",
                        "Baby-born",
                        "Poppenhuis",
                        "Speelauto's en racebanen",
                        "Garages",
                        "Treinen en treinbanen",
                        "Drones",
                        "Kleurboeken",
                        "Prentenboeken",
                        "Fictie boeken",
                        "Bijtringen en rammelaars",
                        "Badspeelgoed",
                        "Stapelspeelgoed",
                        "Vormpjesspeelgoed",
                        "Overig babyspeelgoed",
                        "Lego city",
                        "Lego Ninjago",
                        "Lego Duplo",
                        "K'nex",
                        "BRIO",
                        "Speeltoestellen",
                        "Zandspeelgoed",
                        "Zwembaden",
                        "Rijden speelgoed",
                        "Knutselpakketten",
                        "Tekenen",
                        "Schilderen"
                    };


                for (int j = 1; j <= wc; j++)
                {
                    var wD = ODCL.Where(c => c.W == j).FirstOrDefault();
                    if (wD != null)
                    {
                        t6[j] = new object[] { "week" + wD.W,
                            wD.ct.Where(x => x.C == "Xbox1").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Switch").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "PS4").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Bordspellen").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Kaartspellen").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Partyspellen").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Kinderpuzzels").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "3D puzzels").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Barbie").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Baby-born").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Poppenhuis").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Speelauto's en racebanen").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Garages").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Treinen en treinbanen").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Drones").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Kleurboeken").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Prentenboeken").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Fictie boeken").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Bijtringen en rammelaars").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Badspeelgoed").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Stapelspeelgoed").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Vormpjesspeelgoed").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Overig babyspeelgoed").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Lego city").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Lego Ninjago").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Lego Duplo").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "K'nex").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "BRIO").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Speeltoestellen").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Zandspeelgoed").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Zwembaden").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Rijden speelgoed").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Knutselpakketten").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Tekenen").Select(c => c.T).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Schilderen").Select(c => c.T).FirstOrDefault()


                        };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult WSP(int y) //DATA WEEK CATEGORIES
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<CL>();
            if (t1 != null)
            {
                for (int i = 0; i <= 53; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Week == i) != null)
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                t = GTWS(i, c.SCName, y) //deze functie is anders dan DWCVerkocht(enige wat verandert wordt/ winst ipv sum)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CL
                        {
                            W = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                t = 0m
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CL
                        {
                            W = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var wc = 53;
                var t6 = new object[wc + 1];
                t6[0] = new object[]
                    {
                        "Week",
                        "Xbox1",
                        "Switch",
                        "PS4",
                        "Bordspellen",
                        "Kaartspellen",
                        "Partyspellen",
                        "Kinderpuzzels",
                        "3D puzzels",
                        "Barbie",
                        "Baby-born",
                        "Poppenhuis",
                        "Speelauto's en racebanen",
                        "Garages",
                        "Treinen en treinbanen",
                        "Drones",
                        "Kleurboeken",
                        "Prentenboeken",
                        "Fictie boeken",
                        "Bijtringen en rammelaars",
                        "Badspeelgoed",
                        "Stapelspeelgoed",
                        "Vormpjesspeelgoed",
                        "Overig babyspeelgoed",
                        "Lego city",
                        "Lego Ninjago",
                        "Lego Duplo",
                        "K'nex",
                        "BRIO",
                        "Speeltoestellen",
                        "Zandspeelgoed",
                        "Zwembaden",
                        "Rijden speelgoed",
                        "Knutselpakketten",
                        "Tekenen",
                        "Schilderen"
                    };


                for (int j = 1; j <= wc; j++)
                {
                    var wD = ODCL.Where(c => c.W == j).FirstOrDefault();
                    if (wD != null)
                    {
                        t6[j] = new object[] { "week" + wD.W,
                            wD.ct.Where(x => x.C == "Xbox1").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Switch").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "PS4").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Bordspellen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Partyspellen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Kinderpuzzels").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "3D puzzels").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Barbie").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Baby-born").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Poppenhuis").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Speelauto's en racebanen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Garages").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Treinen en treinbanen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Drones").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Kleurboeken").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Prentenboeken").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Fictie boeken").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Bijtringen en rammelaars").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Badspeelgoed").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Stapelspeelgoed").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Vormpjesspeelgoed").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Overig babyspeelgoed").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Lego city").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Lego Ninjago").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Lego Duplo").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "K'nex").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "BRIO").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Speeltoestellen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Zandspeelgoed").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Zwembaden").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Rijden speelgoed").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Knutselpakketten").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Tekenen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Schilderen").Select(c => c.t).FirstOrDefault()


                        };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult WSR(int y) //DATA WEEK CATEGORIES
        {
            var t1 = (from o in db.OrderDetails
                      where o.year == y
                      select o).ToList();

            var ODCL = new List<CL>();
            if (t1 != null)
            {
                for (int i = 0; i <= 53; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Week == i) != null)
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                t = GTOS(i, c.SCName, y) //deze functie is anders dan DWCVerkocht(enige wat verandert wordt/ omzet ipv sum)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CL
                        {
                            W = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.SubCategories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.SCName,
                                t = 0m
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CL
                        {
                            W = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var wc = 53;
                var t6 = new object[wc + 1];
                t6[0] = new object[]
                    {
                        "Week",
                        "Xbox1",
                        "Switch",
                        "PS4",
                        "Bordspellen",
                        "Kaartspellen",
                        "Partyspellen",
                        "Kinderpuzzels",
                        "3D puzzels",
                        "Barbie",
                        "Baby-born",
                        "Poppenhuis",
                        "Speelauto's en racebanen",
                        "Garages",
                        "Treinen en treinbanen",
                        "Drones",
                        "Kleurboeken",
                        "Prentenboeken",
                        "Fictie boeken",
                        "Bijtringen en rammelaars",
                        "Badspeelgoed",
                        "Stapelspeelgoed",
                        "Vormpjesspeelgoed",
                        "Overig babyspeelgoed",
                        "Lego city",
                        "Lego Ninjago",
                        "Lego Duplo",
                        "K'nex",
                        "BRIO",
                        "Speeltoestellen",
                        "Zandspeelgoed",
                        "Zwembaden",
                        "Rijden speelgoed",
                        "Knutselpakketten",
                        "Tekenen",
                        "Schilderen"
                    };


                for (int j = 1; j <= wc; j++)
                {
                    var wD = ODCL.Where(c => c.W == j).FirstOrDefault();
                    if (wD != null)
                    {
                        t6[j] = new object[] { "week" + wD.W,
                            wD.ct.Where(x => x.C == "Xbox1").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Switch").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "PS4").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Bordspellen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Partyspellen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Kinderpuzzels").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "3D puzzels").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Barbie").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Baby-born").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Poppenhuis").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Speelauto's en racebanen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Garages").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Treinen en treinbanen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Drones").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Kleurboeken").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Prentenboeken").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Fictie boeken").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Bijtringen en rammelaars").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Badspeelgoed").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Stapelspeelgoed").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Vormpjesspeelgoed").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Overig babyspeelgoed").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Lego city").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Lego Ninjago").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Lego Duplo").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "K'nex").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "BRIO").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Speeltoestellen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Zandspeelgoed").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Zwembaden").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Rijden speelgoed").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Knutselpakketten").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Tekenen").Select(c => c.t).FirstOrDefault(),
                            wD.ct.Where(x => x.C == "Schilderen").Select(c => c.t).FirstOrDefault()


                        };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }





        /*----------------------------------------------------------------------------------------------------SC----------------------------------------------------------------------------------------------------*/









        /*-------------------------------------------------------------------------------------------------*/



        //totaal verkocht
        //omzet
        //winst
        //



        /*-----------------------------------------------EXPERIMENTS--------------------------------------------------*/

        public class weeksInString
        {
            public string Day { get; set; }
            public decimal? Total { get; set; }

            //public weeksInString(string d, decimal? t)
            //{
            //    this.Day = d;
            //    this.Total = t;
            //}

            public decimal? total()
            {
                return this.Total;
            }

            public string day()
            {
                return this.Day;
            }


        }

        public class weeks
        {
            public int Week { get; set; }
            public decimal? Total { get; set; }

            public decimal? total()
            {
                return this.Total;
            }

            public int? week()
            {
                return this.Week;
            }
        }

        public string GCategory(int id)
        {
            var CIQ = from t in db.Toy
                      where t.ToysId == id
                      select t.Categories;
            var c = CIQ.FirstOrDefault();
            return c.CName;
        }

        public string GSCategory(int id)
        {
            var CIQ = from t in db.Toy
                      where t.ToysId == id
                      select t.SubCategories;
            var c = CIQ.FirstOrDefault();
            return c.SCName;
        }

        public class weeksT
        {
            public int? week { get; set; }
            public int? quantity { get; set; }
            public int? toysId { get; set; }

        }

        public class weekData
        {
            public int week;
            public weeks[] data;
            public decimal? Total = 0;

            public weekData(int w, weeks[] d)
            {
                this.week = w;
                this.data = d;

            }

            public decimal? total()
            {
                foreach (var t in this.data)
                {
                    this.Total = this.Total + t.Total;
                }
                return this.Total;
            }

            public int Week()
            {
                return this.week;
            }
        }

        public class jsdt
        {
            public int year;
            public int month;
            public jsdt(int y, int m)
            {
                this.year = y;
                this.month = m;
            }
        }

        //parse functie schrijven die datetime neemt en day of year returnt
        public int DOY(DateTime? d)
        {
            var dayofyear = d.Value.DayOfYear;
            return dayofyear;
        }

        //function krijgt week + dag en return jaardag
        public int YDR(int w, int d)
        {
            var day = d;
            for (int i = 2; i < w; i++)
            {
                day = day + 7;
            }
            return day;
        }




        public JsonResult YearData(string F, string SCC)
        {
            var sdy = (from o in db.Order
                       group o by o.OrderDate.Value.Year into b
                       select b.Key).ToList();

            var FY = sdy.FirstOrDefault();
            var LY = sdy.LastOrDefault();
            var YC = sdy.Count();

            //var t1 = (from o in db.OrderDetails
            //          group o by o.year into y
            //          select y).ToList();

            var t2 = (from o in db.OrderDetails
                      select o).ToList();

            var ODCL = new List<CLY>();
            if (SCC == "C")
            {
                if(F == "V")
                {
                    if (t2 != null)
                    {
                        for (int? i = FY; i <= LY; i++)
                        {
                            var TEMPCT = new List<CT>();

                            if (t2.Where(x => x.year == i) != null)
                            {
                                var CC = from C in db.Categories
                                         select C;
                                foreach (var c in CC)
                                {
                                    var T2 = new CT
                                    {
                                        C = c.CName,
                                        T = TCY((int)i, c.CName)
                                    };
                                    TEMPCT.Add(T2);
                                }

                                var TEMPCL = new CLY
                                {
                                    Y = (int)i,
                                    ct = TEMPCT
                                };
                                ODCL.Add(TEMPCL);
                            }
                            else
                            {
                                var CC = from C in db.Categories
                                         select C;
                                foreach (var c in CC)
                                {
                                    var T2 = new CT
                                    {
                                        C = c.CName,
                                        T = 0
                                    };
                                    TEMPCT.Add(T2);
                                }

                                var TEMPCL = new CLY
                                {
                                    Y = (int)i,
                                    ct = TEMPCT
                                };
                                ODCL.Add(TEMPCL);
                            }
                        }
                    }
                    if (ODCL != null)
                    {
                        var t6 = new object[YC + 1];
                        t6[0] = new object[]
                            {
                        "Year",
                        "Bord en Kaartspellen",
                        "Poppen",
                        "Speelvoertuigen",
                        "Video games",
                        "Puzzels",
                        "Lego",
                        "Bouwen",
                        "Boeken",
                        "Hobby en creatief",
                        "Baby en peuter",
                        "Buitenspeelgoed"
                            };

                        var j = 0;
                        foreach (var i in ODCL)
                        {
                            j++;
                            if (i != null)
                            {
                                t6[j] = new object[] { i.Y.ToString(),
                            i.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Poppen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Video games").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Puzzels").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bouwen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Boeken").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Baby en peuter").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.T).FirstOrDefault(),


                        };
                            }
                            else
                            {
                                t6[j] = new object[] { i.Y.ToString(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            }
                        }

                        return new JsonResult
                        {
                            Data = t6,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };

                    }


                }
                else if(F == "W")
                {
                    if (t2 != null)
                    {
                        for (int? i = FY; i <= LY; i++)
                        {
                            var TEMPCT = new List<CT>();

                            if (t2.Where(x => x.year == i) != null)
                            {
                                var CC = from C in db.Categories
                                         select C;
                                foreach (var c in CC)
                                {
                                    var T2 = new CT
                                    {
                                        C = c.CName,
                                        t = PCY((int)i, c.CName)
                                    };
                                    TEMPCT.Add(T2);
                                }

                                var TEMPCL = new CLY
                                {
                                    Y = (int)i,
                                    ct = TEMPCT
                                };
                                ODCL.Add(TEMPCL);
                            }
                            else
                            {
                                var CC = from C in db.Categories
                                         select C;
                                foreach (var c in CC)
                                {
                                    var T2 = new CT
                                    {
                                        C = c.CName,
                                        t = 0
                                    };
                                    TEMPCT.Add(T2);
                                }

                                var TEMPCL = new CLY
                                {
                                    Y = (int)i,
                                    ct = TEMPCT
                                };
                                ODCL.Add(TEMPCL);
                            }
                        }
                    }

                    if (ODCL != null)
                    {
                        var t6 = new object[YC + 1];
                        t6[0] = new object[]
                            {
                        "Year",
                        "Bord en Kaartspellen",
                        "Poppen",
                        "Speelvoertuigen",
                        "Video games",
                        "Puzzels",
                        "Lego",
                        "Bouwen",
                        "Boeken",
                        "Hobby en creatief",
                        "Baby en peuter",
                        "Buitenspeelgoed"
                            };

                        var j = 0;
                        foreach (var i in ODCL)
                        {
                            j++;
                            if (i != null)
                            {
                                t6[j] = new object[] { i.Y.ToString(),
                            i.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Poppen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Video games").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Puzzels").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bouwen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Boeken").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Baby en peuter").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.t).FirstOrDefault(),


                        };
                            }
                            else
                            {
                                t6[j] = new object[] { i.Y.ToString(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            }
                        }

                        return new JsonResult
                        {
                            Data = t6,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    return new JsonResult
                    {
                        Data = null,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            
                else if(F == "O")
                {
                    if (t2 != null)
                    {
                        for (int? i = FY; i <= LY; i++)
                        {
                            var TEMPCT = new List<CT>();

                            if (t2.Where(x => x.year == i) != null)
                            {
                                var CC = from C in db.Categories
                                         select C;
                                foreach (var c in CC)
                                {
                                    var T2 = new CT
                                    {
                                        C = c.CName,
                                        t = RCY((int)i, c.CName)
                                    };
                                    TEMPCT.Add(T2);
                                }

                                var TEMPCL = new CLY
                                {
                                    Y = (int)i,
                                    ct = TEMPCT
                                };
                                ODCL.Add(TEMPCL);
                            }
                            else
                            {
                                var CC = from C in db.Categories
                                         select C;
                                foreach (var c in CC)
                                {
                                    var T2 = new CT
                                    {
                                        C = c.CName,
                                        t = 0
                                    };
                                    TEMPCT.Add(T2);
                                }

                                var TEMPCL = new CLY
                                {
                                    Y = (int)i,
                                    ct = TEMPCT
                                };
                                ODCL.Add(TEMPCL);
                            }
                        }
                    }

                    if (ODCL != null)
                    {
                        var t6 = new object[YC + 1];
                        t6[0] = new object[]
                            {
                        "Year",
                        "Bord en Kaartspellen",
                        "Poppen",
                        "Speelvoertuigen",
                        "Video games",
                        "Puzzels",
                        "Lego",
                        "Bouwen",
                        "Boeken",
                        "Hobby en creatief",
                        "Baby en peuter",
                        "Buitenspeelgoed"
                            };

                        var j = 0;
                        foreach (var i in ODCL)
                        {
                            j++;
                            if (i != null)
                            {
                                t6[j] = new object[] { i.Y.ToString(),
                            i.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Poppen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Video games").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Puzzels").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bouwen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Boeken").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Baby en peuter").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.t).FirstOrDefault(),


                        };
                            }
                            else
                            {
                                t6[j] = new object[] { i.Y.ToString(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            }

                        }

                        return new JsonResult
                        {
                            Data = t6,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    return new JsonResult
                    {
                        Data = null,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }                
            }
            else if (SCC == "SC")
            {
                if (F == "V")
                {
                    if (t2 != null)
                    {
                        for (int? i = FY; i <= LY; i++)
                        {
                            var TEMPCT = new List<CT>();

                            if (t2.Where(x => x.year == i) != null)
                            {
                                var CC = from C in db.Categories
                                         select C;
                                foreach (var c in CC)
                                {
                                    var T2 = new CT
                                    {
                                        C = c.CName,
                                        T = TCY((int)i, c.CName)
                                    };
                                    TEMPCT.Add(T2);
                                }

                                var TEMPCL = new CLY
                                {
                                    Y = (int)i,
                                    ct = TEMPCT
                                };
                                ODCL.Add(TEMPCL);
                            }
                            else
                            {
                                var CC = from C in db.Categories
                                         select C;
                                foreach (var c in CC)
                                {
                                    var T2 = new CT
                                    {
                                        C = c.CName,
                                        T = 0
                                    };
                                    TEMPCT.Add(T2);
                                }

                                var TEMPCL = new CLY
                                {
                                    Y = (int)i,
                                    ct = TEMPCT
                                };
                                ODCL.Add(TEMPCL);
                            }
                        }
                    }
                    if (ODCL != null)
                    {
                        var t6 = new object[YC + 1];
                        t6[0] = new object[]
                            {
                        "Year",
                        "Bord en Kaartspellen",
                        "Poppen",
                        "Speelvoertuigen",
                        "Video games",
                        "Puzzels",
                        "Lego",
                        "Bouwen",
                        "Boeken",
                        "Hobby en creatief",
                        "Baby en peuter",
                        "Buitenspeelgoed"
                            };

                        var j = 0;
                        foreach (var i in ODCL)
                        {
                            j++;
                            if (i != null)
                            {
                                t6[j] = new object[] { i.Y.ToString(),
                            i.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Poppen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Video games").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Puzzels").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bouwen").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Boeken").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Baby en peuter").Select(c => c.T).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.T).FirstOrDefault(),


                        };
                            }
                            else
                            {
                                t6[j] = new object[] { i.Y.ToString(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            }
                        }

                        return new JsonResult
                        {
                            Data = t6,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };

                    }


                }

                else if (F == "W")
                {
                    if (t2 != null)
                    {
                        for (int? i = FY; i <= LY; i++)
                        {
                            var TEMPCT = new List<CT>();

                            if (t2.Where(x => x.year == i) != null)
                            {
                                var CC = from C in db.Categories
                                         select C;
                                foreach (var c in CC)
                                {
                                    var T2 = new CT
                                    {
                                        C = c.CName,
                                        t = PCY((int)i, c.CName)
                                    };
                                    TEMPCT.Add(T2);
                                }

                                var TEMPCL = new CLY
                                {
                                    Y = (int)i,
                                    ct = TEMPCT
                                };
                                ODCL.Add(TEMPCL);
                            }
                            else
                            {
                                var CC = from C in db.Categories
                                         select C;
                                foreach (var c in CC)
                                {
                                    var T2 = new CT
                                    {
                                        C = c.CName,
                                        t = 0
                                    };
                                    TEMPCT.Add(T2);
                                }

                                var TEMPCL = new CLY
                                {
                                    Y = (int)i,
                                    ct = TEMPCT
                                };
                                ODCL.Add(TEMPCL);
                            }
                        }
                    }

                    if (ODCL != null)
                    {
                        var t6 = new object[YC + 1];
                        t6[0] = new object[]
                            {
                        "Year",
                        "Bord en Kaartspellen",
                        "Poppen",
                        "Speelvoertuigen",
                        "Video games",
                        "Puzzels",
                        "Lego",
                        "Bouwen",
                        "Boeken",
                        "Hobby en creatief",
                        "Baby en peuter",
                        "Buitenspeelgoed"
                            };

                        var j = 0;
                        foreach (var i in ODCL)
                        {
                            j++;
                            if (i != null)
                            {
                                t6[j] = new object[] { i.Y.ToString(),
                            i.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Poppen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Video games").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Puzzels").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bouwen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Boeken").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Baby en peuter").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.t).FirstOrDefault(),


                        };
                            }
                            else
                            {
                                t6[j] = new object[] { i.Y.ToString(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            }
                        }

                        return new JsonResult
                        {
                            Data = t6,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    return new JsonResult
                    {
                        Data = null,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

                else if (F == "O")
                {
                    if (t2 != null)
                    {
                        for (int? i = FY; i <= LY; i++)
                        {
                            var TEMPCT = new List<CT>();

                            if (t2.Where(x => x.year == i) != null)
                            {
                                var CC = from C in db.Categories
                                         select C;
                                foreach (var c in CC)
                                {
                                    var T2 = new CT
                                    {
                                        C = c.CName,
                                        t = RCY((int)i, c.CName)
                                    };
                                    TEMPCT.Add(T2);
                                }

                                var TEMPCL = new CLY
                                {
                                    Y = (int)i,
                                    ct = TEMPCT
                                };
                                ODCL.Add(TEMPCL);
                            }
                            else
                            {
                                var CC = from C in db.Categories
                                         select C;
                                foreach (var c in CC)
                                {
                                    var T2 = new CT
                                    {
                                        C = c.CName,
                                        t = 0
                                    };
                                    TEMPCT.Add(T2);
                                }

                                var TEMPCL = new CLY
                                {
                                    Y = (int)i,
                                    ct = TEMPCT
                                };
                                ODCL.Add(TEMPCL);
                            }
                        }
                    }

                    if (ODCL != null)
                    {
                        var t6 = new object[YC + 1];
                        t6[0] = new object[]
                            {
                        "Year",
                        "Bord en Kaartspellen",
                        "Poppen",
                        "Speelvoertuigen",
                        "Video games",
                        "Puzzels",
                        "Lego",
                        "Bouwen",
                        "Boeken",
                        "Hobby en creatief",
                        "Baby en peuter",
                        "Buitenspeelgoed"
                            };

                        var j = 0;
                        foreach (var i in ODCL)
                        {
                            j++;
                            if (i != null)
                            {
                                t6[j] = new object[] { i.Y.ToString(),
                            i.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Poppen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Video games").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Puzzels").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Lego").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Bouwen").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Boeken").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Baby en peuter").Select(c => c.t).FirstOrDefault(),
                            i.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.t).FirstOrDefault(),


                        };
                            }
                            else
                            {
                                t6[j] = new object[] { i.Y.ToString(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            }

                        }

                        return new JsonResult
                        {
                            Data = t6,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    return new JsonResult
                    {
                        Data = null,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }



            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        /*-------------------Day-------------------*/
        public int TCD(int Y, string CName, int M, int D) //get total category(sales)                   //////////////IIIIIIIIIII check
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = (from od in db.OrderDetails
                     where od.year == Y && od.CName == CName && od.Month == M && od.Day == D
                     select od.Quantity).Sum(); // ToList(); ///? quantity.sum != quantity * sum

            if (T != null)
            {
                //var R = 0;
                //foreach(var I in T)
                //{
                //    R += (int)I;
                //}
                var R = (int)T;
                return R;
            }
            else
            {
                var R = 0;
                return R;
            }
        }

        public decimal? WCD(int Y, string CName, int M, int D) //get total winst category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = from od in db.OrderDetails
                    where od.year == Y && od.CName == CName && od.Month == M && od.Day == D
                    select od;
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            if (T != null)
            {
                decimal? R = 0;
                foreach (var i in T)
                {
                    R += (i.UnitPrice * i.Quantity) * 1.08m; //8% winst(m = decimal representatie)
                }
                return R;


            }
            else
            {
                decimal? R = 0;
                return R;
            }
        }

        public decimal? OCD(int Y, string CName, int M, int D) //get total omzet category
        {
            //returnt lijst met alle records waarbij de week en de cname zoals assignd is
            var T = from od in db.OrderDetails
                    where od.year == Y && od.CName == CName && od.Month == M && od.Day == D
                    select od;
            //functie die toy, quantity, prijs neemt en totaal uitrekent

            if (T != null)
            {
                decimal? R = 0;
                foreach (var i in T)
                {
                    R += (i.UnitPrice * i.Quantity);
                }
                return R;


            }
            else
            {
                decimal? R = 0;
                return R;
            }
        }
        /*-------------------Day-------------------*/

        public JsonResult DDMCV(int y, string m)
        {
            int M = DateTime.ParseExact(m, "MMMM", CultureInfo.InvariantCulture).Month;
            int days = DateTime.DaysInMonth(y, M);


            var t1 = (from o in db.OrderDetails
                      where o.year == y && o.Month == M
                      select o).ToList();

            var ODCL = new List<CLD>();
            if (t1 != null)
            {
                for (int i = 0; i <= days; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Day == i) != null)
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                T = TCD(y, c.CName, M, i)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLD
                        {
                            D = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                T = 0
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLD
                        {
                            D = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[days + 1];

                t6[0] = new object[]
                  {
                    "Day",
                    "Bord en Kaartspellen",
                    "Poppen",
                    "Speelvoertuigen",
                    "Video games",
                    "Puzzels",
                    "Lego",
                    "Bouwen",
                    "Boeken",
                    "Hobby en creatief",
                    "Baby en peuter",
                    "Buitenspeelgoed"
                  };
                for (int i = 1; i <= days; i++)
                {
                    var DD = ODCL.Where(a => a.D.Equals(i)).FirstOrDefault();
                    if (DD != null)
                    {
                        t6[i] = new object[] { i.ToString(),
                            DD.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.T).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Poppen").Select(c => c.T).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.T).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Video games").Select(c => c.T).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Puzzels").Select(c => c.T).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Lego").Select(c => c.T).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Bouwen").Select(c => c.T).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Boeken").Select(c => c.T).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.T).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Baby en peuter").Select(c => c.T).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.T).FirstOrDefault(),


                        };
                    }
                    else
                    {
                        t6[i] = new object[] { i.ToString(), 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult DDMCW(int y, string m)
        {
            int M = DateTime.ParseExact(m, "MMMM", CultureInfo.InvariantCulture).Month;
            int days = DateTime.DaysInMonth(y, M);


            var t1 = (from o in db.OrderDetails
                      where o.year == y && o.Month == M
                      select o).ToList();

            var ODCL = new List<CLD>();
            if (t1 != null)
            {
                for (int i = 0; i <= days; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Day == i) != null)
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = WCD(y, c.CName, M, i)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLD
                        {
                            D = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = 0m
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLD
                        {
                            D = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[days + 1];

                t6[0] = new object[]
                  {
                    "Day",
                    "Bord en Kaartspellen",
                    "Poppen",
                    "Speelvoertuigen",
                    "Video games",
                    "Puzzels",
                    "Lego",
                    "Bouwen",
                    "Boeken",
                    "Hobby en creatief",
                    "Baby en peuter",
                    "Buitenspeelgoed"
                  };
                for (int i = 1; i <= days; i++)
                {
                    var DD = ODCL.Where(a => a.D.Equals(i)).FirstOrDefault();
                    if (DD != null)
                    {
                        t6[i] = new object[] { i.ToString(),
                            DD.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Poppen").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Video games").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Puzzels").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Lego").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Bouwen").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Boeken").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Baby en peuter").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.t).FirstOrDefault(),


                        };
                    }
                    else
                    {
                        t6[i] = new object[] { i.ToString(), 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult DDMCO(int y, string m)
        {
            int M = DateTime.ParseExact(m, "MMMM", CultureInfo.InvariantCulture).Month;
            int days = DateTime.DaysInMonth(y, M);


            var t1 = (from o in db.OrderDetails
                      where o.year == y && o.Month == M
                      select o).ToList();

            var ODCL = new List<CLD>();
            if (t1 != null)
            {
                for (int i = 0; i <= days; i++)
                {
                    var TEMPCT = new List<CT>();

                    if (t1.Where(x => x.Day == i) != null)
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = OCD(y, c.CName, M, i)
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLD
                        {
                            D = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                    else
                    {
                        var CC = from C in db.Categories
                                 select C;
                        foreach (var c in CC)
                        {
                            var T2 = new CT
                            {
                                C = c.CName,
                                t = 0m
                            };
                            TEMPCT.Add(T2);
                        }

                        var TEMPCL = new CLD
                        {
                            D = i,
                            ct = TEMPCT
                        };
                        ODCL.Add(TEMPCL);
                    }
                }
            }

            if (ODCL != null)
            {
                var t6 = new object[days + 1];

                t6[0] = new object[]
                  {
                    "Day",
                    "Bord en Kaartspellen",
                    "Poppen",
                    "Speelvoertuigen",
                    "Video games",
                    "Puzzels",
                    "Lego",
                    "Bouwen",
                    "Boeken",
                    "Hobby en creatief",
                    "Baby en peuter",
                    "Buitenspeelgoed"
                  };
                for (int i = 1; i <= days; i++)
                {
                    //string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                    var DD = ODCL.Where(a => a.D.Equals(i)).FirstOrDefault();
                    if (DD != null)
                    {
                        t6[i] = new object[] { i.ToString(),
                            DD.ct.Where(x => x.C == "Bord en Kaartspellen").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Poppen").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Speelvoertuigen").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Video games").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Puzzels").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Lego").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Bouwen").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Boeken").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Hobby en creatief").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Baby en peuter").Select(c => c.t).FirstOrDefault(),
                            DD.ct.Where(x => x.C == "Buitenspeelgoed").Select(c => c.t).FirstOrDefault(),


                        };
                    }
                    else
                    {
                        t6[i] = new object[] { i.ToString(), 0 };
                    }

                }
                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /*-------------------------------------------------------------------------------------------------*/

        public JsonResult SalesDataYearWise()
        {

            var sdy = from o in db.Order
                      group o by o.OrderDate.Value.Year into b
                      //group o by Convert.ToDateTime(o.OrderDate).ToString("yyyy") into b
                      select new
                      {
                          year = b.Key,
                          totaal = b.Sum(a => a.Total)

                      };
            if (sdy != null)
            {
                int sdyCount = sdy.Count() + 1;
                var chartData = new object[sdyCount];
                chartData[0] = new object[]
                {
                    "Year",
                    "Total"
                };
                int j = 0;
                foreach (var i in sdy)
                {
                    j++;
                    chartData[j] = new object[] { i.year.ToString(), i.totaal };
                }
                return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public JsonResult SalesDataMonthWise(int y)
        {
            var t4 = (from o in db.Order
                      where o.OrderDate.Value.Year.Equals(y)
                      //where Convert.ToDateTime(o.OrderDate).ToString("yyyy") == year
                      group o by o.OrderDate.Value.Month into b
                      //group o by Convert.ToDateTime(o.OrderDate).ToString("MMMM") into b
                      select new
                      {
                          Month = b.Key,
                          Total = b.Sum(a => a.Total)
                      });


            if (t4 != null)
            {
                var chartData = new object[12 + 1];

                chartData[0] = new object[]
                  {
                    "Month",
                    "Total"
                  };
                for (int i = 1; i <= 12; i++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                    var monthData = t4.Where(a => a.Month.Equals(i)).FirstOrDefault();
                    if (monthData != null)
                    {
                        chartData[i] = new object[] { monthName, monthData.Total };
                    }
                    else
                    {
                        chartData[i] = new object[] { monthName, 0 };
                    }

                }
                return new JsonResult
                {
                    Data = chartData,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SalesDataDayWise(int y, string m)
        {
            int mNumber = DateTime.ParseExact(m, "MMMM", CultureInfo.InvariantCulture).Month;
            //int week = DateTime.ParseExact(w, "WWWW", CultureInfo.InvariantCulture).DayOfWeek
            int days = DateTime.DaysInMonth(y, mNumber);
            //string year = y.ToString();
            List<Order> O = new List<Order>();
            O = (from o in db.Order
                 where o.OrderDate.Value.Year.Equals(y)
                 && o.OrderDate.Value.Month.Equals(mNumber)
                 //where Convert.ToDateTime(o.OrderDate).ToString("yyyy") == year
                 //&& Convert.ToDateTime(o.OrderDate).ToString("MMMM") == m
                 select o).ToList();
            if (O != null)
            {
                var chartData = new object[days + 1];
                chartData[0] = new object[]
                {
                    "Month",
                    "Total"
                };
                for (int i = 1; i <= days; i++)
                {
                    var daysData = O.Where(a => a.OrderDate.Value.Day.Equals(i)).FirstOrDefault(); //Convert.ToDateTime(a.OrderDate).ToString("dd").Equals(i)
                    if (daysData != null)
                    {
                        chartData[i] = new object[] { i.ToString(), daysData.Total };
                    }
                    else
                    {
                        chartData[i] = new object[] { i.ToString(), 0 };
                    }
                }
                return new JsonResult
                {
                    Data = chartData,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult DataWeekWise(int y) //error linq doesnt support value.dayofyear???// error linq doesnt support system.tostring? use sqlfunction?
        {
            var t1 = (from o in db.Order
                      where o.OrderDate.Value.Year.Equals(y)
                      group o by o.week into b
                      select new weeks
                      {
                          Week = (int)b.Key,
                          Total = b.Sum(a => a.Total)
                      });

            if (t1 != null)
            {
                var t2 = new List<weeks>();
                for (int i = 1; i <= 53; i++)
                {
                    weeks TEMPW = new weeks { };
                    if (t1.Where(w => w.Week == i) != null)
                    {
                        TEMPW.Week = i;
                        TEMPW.Total = (from o in db.Order
                                       where o.OrderDate.Value.Year.Equals(y) && o.week == i
                                       group o by o.week into T
                                       select T.Sum(x => x.Total)

                                       ).ToList().FirstOrDefault();

                        t2.Add(TEMPW);
                    }
                    else
                    {
                        TEMPW.Week = i;
                        TEMPW.Total = 0;
                        t2.Add(TEMPW);
                    }
                }
                var wc = 53;

                var t6 = new object[wc + 1];
                t6[0] = new object[]
                    {
                        "Week",
                        "Total"
                    };


                for (int j = 1; j <= wc; j++)
                {
                    var wt = t2.Where(c => c.Week == j).FirstOrDefault();
                    if (wt != null)
                    {
                        t6[j] = new object[] { "week" + wt.Week, wt.Total };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //filter idee: filterlist in loop generated by javascript radio checker?

        /*-----------------------------------------------EXPERIMENTS--------------------------------------------------*/




    }

}


