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
using Toymania.Services;

namespace Toymania.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            OrdersService OrderService = new OrdersService();
            var Model = new OrdersViewModel
            {
                AllOrders = OrderService.GetOrders(this.HttpContext),
                InProgressOrders = OrderService.GetOrders(this.HttpContext, "In Progress"),
                CompletedOrders = OrderService.GetOrders(this.HttpContext, "Completed")
            };

            return View(Model);
        }

        public ActionResult Detail(int id, string Status)
        {
            var OrderService = new OrdersService();
            if(Status != null)
            {
                var Model = new OrderDetailViewModel
                {
                    OrderDetails = OrderService.GetOrderDetails(id, Status)
                };
                return View(Model);
            }
            else
            {
                var Model = new OrderDetailViewModel
                {
                    OrderDetails = OrderService.GetOrderDetails(id)
                };
                return View(Model);
            }

        }


    }
}