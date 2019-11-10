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
        public List<Categories> Categories { get; set; }
        public List<SubCategories> SubCategories { get; set; }

        public string SelectedCategory { get; set; }
        public string SelectedSubCategory { get; set; }
        public string Search { get; set; }

        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int PageSize { get; set; }

        //public ICollection<Toy> T { get; set; }
        public IPagedList<Toy> Toys { get; set; }
    }
}