using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toymania.Models;

namespace Toymania.ViewModels
{
    public class Toymanager
    {
        public List<Toy> toys { get; set; }
        public List<Toy> deleted { get; set; }
    }
}