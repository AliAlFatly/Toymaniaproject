using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toymania.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc.Html;
using System.Web.Mvc;

namespace Toymania.ViewModels
{
    public class OrderDetailViewModel
    {
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}