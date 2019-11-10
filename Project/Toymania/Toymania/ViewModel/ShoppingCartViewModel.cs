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
    public class ShoppingCartViewModel
    {
        [Key]
        public List<Cart> CartToy { get; set; } //cart items
        public bool U { get; set; }
        public decimal CartTotal { get; set; }
    }
}