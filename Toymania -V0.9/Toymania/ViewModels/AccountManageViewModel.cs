using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toymania.Models;


namespace Toymania.ViewModels
{
    public class AccountManageViewModel
    {
        public List<ApplicationUser> c { get; set; }
        public List<ApplicationUser> a { get; set; }
    }
}