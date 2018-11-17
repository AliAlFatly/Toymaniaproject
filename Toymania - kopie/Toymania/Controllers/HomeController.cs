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

namespace Toymania.Controllers
{
    public class HomeController : Controller
    {
        //private readonly UserManager<ApplicationUser> _userManager;

        //public HomeController(UserManager<ApplicationUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        public ActionResult Index()
        {
            //ViewBag.userId = _userManager.GetEmail(HttpContext.User);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Help()
        {
            ViewBag.Message = "Your Help page";

            return View();
        }
    }
}