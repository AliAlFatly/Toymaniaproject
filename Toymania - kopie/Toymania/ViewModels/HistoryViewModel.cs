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
    public class HistoryViewModel
    {
        public List<OrderDetails> IPOO { get; set; } //In progress orderdetails
        public List<OrderDetails> COO { get; set; } //Completed orderdetails
    }
}