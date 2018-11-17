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


            var OVM = new OrdersViewModel
            {
                Order = O
            };

            return View(OVM);
        }

        public ActionResult Detail(int id)
        {
            var o = new Orders();

            var ODTemp = o.GD(id);

            var ODVM = new OrderDetailViewModel
            {
                OD = ODTemp
            };

            return View(ODVM);
        }


    }
}