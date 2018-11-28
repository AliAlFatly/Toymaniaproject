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

namespace Toymania.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders
        TSE15 db = new TSE15();
        public ActionResult Index()
        {
            Orders o = new Orders();
            List<Order> O = o.GO(this.HttpContext);
            List<Order> IP = o.GOS(this.HttpContext, "In Progress");
            List<Order> C = o.GOS(this.HttpContext, "Completed");


            var OVM = new OrdersViewModel
            {
                O = O,
                IP = IP,
                C = C
            };

            return View(OVM);
        }

        public ActionResult Detail(int id, string S)
        {
            var o = new Orders();

            if( S != null)
            {
                var ODTemp = o.GDS(id, S);
                var ODVM = new OrderDetailViewModel
                {
                    OD = ODTemp
                };
                return View(ODVM);
            }
            else
            {
                var ODTemp = o.GD(id);
                var ODVM = new OrderDetailViewModel
                {
                    OD = ODTemp
                };
                return View(ODVM);
            }

        }


    }
}