﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toymania.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc.Ajax;


namespace Toymania.ViewModels
{
    public class OrdersViewModel
    {
        public List<Order> Order { get; set; } //ICollection of orders

    }
}