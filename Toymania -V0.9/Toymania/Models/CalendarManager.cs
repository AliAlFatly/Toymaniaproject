using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
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

namespace Toymania.Models
{
    public class CalendarManager
    {
        public class A
        {
            //    public int a;
            //    public string b;
            //    public A(int a, string b)
            //    {
            //        this.a = a;
            //        this.b = b;
            //    }

            //}


            //public List<List<A>, List<string>, List<string>, List<string>> TC() //List<List<int,string>,List<string>,List<string>,List<string>>
            //{

            //}

            //public List<A> TDay()
            //{

            //}

            public List<string> TW()
            {
                var weken = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
                                                "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
                                                "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
                                                "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
                                                "41", "42", "43", "44", "45", "46", "47", "48", "49", "50",
                                                "51", "52"
                                             };
                return weken;
            }

            public List<string> TM()
            {
                var maanden = new List<string> { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september",
                                                "oktober", "november", "december"
                                             };
                return maanden;

            }

        }
    }
}