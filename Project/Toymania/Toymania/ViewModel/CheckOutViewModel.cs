using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toymania.Models;

namespace Toymania.ViewModel
{
    public class CheckOutViewModel
    {
        public Order Order { get; set; }
        public bool IsLoggedIn { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}