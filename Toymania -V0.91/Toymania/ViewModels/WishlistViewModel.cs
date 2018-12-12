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
    public class WishlistViewModel
    {
        public List<Wishlist> WT { get; set; } //get wishlist in a list to print in a table as a wishlist toy
    }
}