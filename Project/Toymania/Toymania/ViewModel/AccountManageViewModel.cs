using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toymania.Models;


namespace Toymania.ViewModels
{
    public class AccountManageViewModel
    {
        public List<ApplicationUser> Customer { get; set; }
        public List<ApplicationUser> Admin { get; set; }
    }
}