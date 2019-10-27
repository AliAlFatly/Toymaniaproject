using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toymania.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Toymania.ViewModels
{
    public class TrackerViewModel
    {
        public List<OrderDetails> IS { get; set; }
        public List<OrderDetails> S { get; set; }
        public List<OrderDetails> C { get; set; }
    }
}