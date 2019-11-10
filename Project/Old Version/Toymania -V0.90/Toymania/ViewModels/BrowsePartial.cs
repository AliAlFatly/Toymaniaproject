using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toymania.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PagedList.Mvc;
using PagedList;

namespace Toymania.ViewModels
{
    public class BrowsePartial
    {
        [Key]
        public List<Categories> C { get; set; }
        public ICollection<SubCategories> SC { get; set; }
        //public ICollection<Toy> T { get; set; }
        public IPagedList<Toy> T { get; set; }
        public int? PC { get; set; }
    }
}