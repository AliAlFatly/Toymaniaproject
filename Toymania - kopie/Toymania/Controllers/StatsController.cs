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
using System.Linq.Expressions;
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



        TSE15 db = new TSE15();

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
            public int Day { get; set; }
            public decimal? Total { get; set; }

            //public weeks(int d, decimal? t)
            //{
            //    this.Day = d;
            //    this.Total = t;
            //}

            public decimal? total()
            {
                return this.Total;
            }

            public int day()
            {
                return this.Day;
            }

            

        }

        public List<weeks> DayOfYear(IQueryable<weeksInString> w) //deze functie moet een helejaar returnen
        {
            DateTime d = Convert.ToDateTime(w.First().Day);
            var year = d.Year;

            if(DateTime.IsLeapYear(year))
            {
                var wk = new List<weeks>();
                for (int i = 1; i <= 366; i++)
                {
                    
                    weeksInString TempWIS = w.Where(a => Convert.ToInt32(a.Day) == i).FirstOrDefault();
                    if(TempWIS != null)
                    {
                        weeks TempW = new weeks
                        {
                            Day = i,
                            Total = TempWIS.Total
                        };
                        wk.Add(TempW);
                    }
                    else
                    {
                        weeks TempW = new weeks
                        {
                            Day = i,
                            Total = 0
                        };
                        wk.Add(TempW);
                    }                    
                }
                return wk;
            }
            else
            {
                var wk = new List<weeks>();
                for (int i = 1; i <= 365; i++)
                {

                    weeksInString TempWIS = w.Where(a => Convert.ToInt32(a.Day) == i).FirstOrDefault();
                    if (TempWIS != null)
                    {
                        weeks TempW = new weeks
                        {
                            Day = i,
                            Total = TempWIS.Total
                        };
                        wk.Add(TempW);
                    }
                    else
                    {
                        weeks TempW = new weeks
                        {
                            Day = i,
                            Total = 0
                        };
                        wk.Add(TempW);
                    }


                }
                return wk;
            }
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
            for (int i = 1; i < w; i++)
            {
                day = day + 7;
            }

            return day;
        }

        public ActionResult Index()
        {
            return View();
        }

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
                      //where Convert.ToDateTime(o.OrderDate).ToString("yyyy") == year
                      group o by o.OrderDate.Value.ToString("dd-MM-yyyy") into b
                      //group o by DOY(o.OrderDate) into b
                      //group o by o.OrderDate.Value.DayOfYear into b
                      //group o by SqlFunctions.StringConvert(("dd-MM-yyyy")o.OrderDate.Value.Day) into b
                      //group o by o.OrderDate.Value.Day.ToString("dd-MM-yyyy") into b
                      //group o by Convert.ToDateTime(o.OrderDate).ToString("dd-MM-yyyy") into b
                      //group o by Convert.ToDateTime(o.OrderDate).ToString("MMMM") into b
                      select new weeksInString          //(b.Key, b.Sum(a => a.Total)));
                      {
                          Day = b.Key,
                          Total = b.Sum(a => a.Total)
                      });

            

            var t4 = DayOfYear(t1);
            
            
            if (t4 != null)
            {
                //var yearnumber = t4.Count();
                var t5 = new List<weekData>();                
                for (int i = 1; i <= 52; i++)
                {
                    weeks[] a = new weeks[7];
                    a[0] = t4.Where(b => b.Day.Equals(YDR(i, 1))).FirstOrDefault();
                    a[1] = t4.Where(b => b.Day.Equals(YDR(i, 2))).FirstOrDefault();
                    a[2] = t4.Where(b => b.Day.Equals(YDR(i, 3))).FirstOrDefault();
                    a[3] = t4.Where(b => b.Day.Equals(YDR(i, 4))).FirstOrDefault();
                    a[4] = t4.Where(b => b.Day.Equals(YDR(i, 5))).FirstOrDefault();
                    a[5] = t4.Where(b => b.Day.Equals(YDR(i, 6))).FirstOrDefault();
                    a[6] = t4.Where(b => b.Day.Equals(YDR(i, 7))).FirstOrDefault();                    
                    DateTime d = Convert.ToDateTime(t1.First().Day);
                    var year = d.Year;
                    if (i == 52 && DateTime.IsLeapYear(year))
                    {
                        weeks[] a3 = new weeks[3];
                        a3[0] = t4.Where(b => b.Day.Equals(YDR(i + 1, 1))).FirstOrDefault();
                        a3[1] = t4.Where(b => b.Day.Equals(YDR(i + 1, 2))).FirstOrDefault();
                        a3[2] = t4.Where(b => b.Day.Equals(YDR(i + 1, 3))).FirstOrDefault();
                        weekData t2 = new weekData(i, a);
                        weekData ta3 = new weekData(i + 1, a3);
                        t5.Add(t2);
                        t5.Add(ta3);
                    }
                    else if(i == 52 && !DateTime.IsLeapYear(year))
                    {
                        weeks[] a2 = new weeks[2];
                        a2[0] = t4.Where(b => b.Day.Equals(YDR(i + 1, 1))).FirstOrDefault();
                        a2[1] = t4.Where(b => b.Day.Equals(YDR(i + 1, 2))).FirstOrDefault();
                        weekData t2 = new weekData(i, a);
                        weekData ta2 = new weekData(i + 1, a2);
                        t5.Add(t2);
                        t5.Add(ta2);
                    }
                    else
                    {
                        weekData t2 = new weekData(i, a);
                        t5.Add(t2);
                    }
                    
                }

                var t6 = new object[52];
                t6 = new object[]
                    {
                        "week",
                        "Total"
                    };
                for (int j = 1; j <= 52; j++)
                {
                    var weeksdata = t5.Where(c => c.week.Equals(j)).FirstOrDefault();
                    var wt = weeksdata.data;
                    if (wt != null)
                    {
                        t6[j] = new object[] { "week" + weeksdata.week, weeksdata.total() };
                    }

                }

                return new JsonResult
                {
                    Data = t6,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                //if( yearnumber > 365)
                //{
                //    for (int i = 1; i <= 366; i++)
                //    {

                //    }
                //    //52 keer 7 items toevoegen
                //    //list van weeknr en daarachter arrays van 7 items met dag + totaal
                //    //items toevoegen 7 per keer
                //}
                //else
                //{
                //    for (int i = 1; i <= 365; i++)
                //    {

                //    }
                //}
            }
            return new JsonResult
            {
                Data = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult Diagram1()
        {
            int year = 2018;                                //I
            var KDY = new DateTime(01 / 01 / year + 1);
            var GDY = new DateTime(31 / 12 / year - 1);
            var t4 = from o in db.Order
                     group o by Convert.ToDateTime(o.OrderDate).ToString("MMMM") into b
                     select new
                     {
                         month = b.Key,
                         total = b.Sum(a => a.Total)
                     };
            if (t4 != null)
            {
                var chartData = new object[t4.Count() + 1];
                chartData[0] = new object[]
                {
                    "month",
                    "total"
                };
                int j = 0;
                foreach (var i in t4)
                {
                    j++;
                    chartData[j] = new object[] { i.month.ToString(), i.total };
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



            //List<string> LT = new List<string>();
            //List<decimal?> LT2 = new List<decimal?>();

            //foreach (var item in test3)
            //{
            //    LT.Add(item.Key);
            //    foreach (var i in item)
            //    {
            //        foreach (var i2 in i)
            //        {
            //            LT2.Add(i2);
            //        }
            //    }
            //}

            //var c = new Chart(width: 800, height: 200).AddSeries(
            //    chartType: "colomn",
            //    xValue: LT,
            //    yValues: LT2).Write();
            //ViewBag.c = c;
            //ViewData["c"] = c;
            //return null;
        }
        
        //test2
        public ActionResult IndexT(int y)
        {
            // int year = y
            int year = 2018;                                //I
            var KDY = new DateTime(01 / 01 / year + 1);
            var GDY = new DateTime(31 / 12 / year - 1);

            //var test = from od in db.OrderDetails
            //           from o in db.Order
            //           where od.Order.OrderDate >  date1 & od.Order.OrderDate < date2 & o.OrderDate > date1 & o.OrderDate < date2
            //           select [new list]

            //maanden selecteren 
            var Test1 = from M in
                     (from m in db.Order
                      where m.OrderDate > GDY & m.OrderDate < KDY
                      select m.OrderDate).ToList()
                        select Convert.ToDateTime(M).ToString("MMMM");




            var test2 = from od in db.OrderDetails
                        from o in db.Order
                        where od.Order.OrderDate > GDY & od.Order.OrderDate < KDY & o.OrderDate > GDY & o.OrderDate < KDY
                        select new { detailid = od.OrderDetailId, month = Convert.ToDateTime(o.OrderDate).ToString("MMMM") };

            //per jaarsmaanden totaal omzet
            // select sum from o.total, group by convert to month o.date
            var JMTO = from o in db.Order
                       where o.OrderDate > GDY & o.OrderDate < KDY
                       group o.Total by Convert.ToDateTime(o.OrderDate).ToString("MMMM") into b
                       select new { maand = b, totaal = b.Sum(), };


            var test3 = from o in db.Order
                        let m = from oo in db.Order
                                where oo.OrderDate > GDY & oo.OrderDate < KDY
                                group oo.Total by Convert.ToDateTime(oo.OrderDate).ToString("MMMM") into b
                                select b.Sum()
                        group m by Convert.ToDateTime(o.OrderDate).ToString("MMMM") into cp
                        select cp;



            List<string> LT = new List<string>();
            List<decimal?> LT2 = new List<decimal?>();

            foreach (var item in test3)
            {
                LT.Add(item.Key);
                foreach (var i in item)
                {
                    foreach (var i2 in i)
                    {
                        LT2.Add(i2);
                    }
                }
            }

            var c = new Chart(width: 800, height: 200).AddSeries(
                chartType: "colomn",
                xValue: LT,
                yValues: LT2).Write();

            ViewBag.d = c;
            ViewData["d"] = c;



            return View();
        }
    }

}


// filter op maanden, weken(jaar basis) weeksdagen, maanddagen?
// op category/sub category
// omzet + winst
// aantal verkocht
// 
//decimal Jan = 0;
//decimal feb = 0;
//decimal mar = 2;
//decimal apr = 4;
//var a = new Chart(width: 800, height: 200).AddSeries(
//    chartType: "column",
//    xValue: new[] { "Jan", "Feb", "Mar", "apr" },
//    yValues: new[] { Jan, feb, mar, apr }).Write("png");
//ViewBag.d = a;
//ViewData["d"] = a;

// nav bar -> diagram + pie chart
//diagram filter based on dropdown menu time + object
// piechart filter = select sub category or category
