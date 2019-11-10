using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toymania.Models;

namespace Toymania.ViewModels
{
    public class Recommended
    {
        public Toy Toy { get; set; }
        public List<Toy> RecommendedToys { get; set; }
    }
}