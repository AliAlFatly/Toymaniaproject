﻿using System;
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
        public List<Cart> CT { get; set; } //cart items

        public decimal CartTotal { get; set; }
    }
}