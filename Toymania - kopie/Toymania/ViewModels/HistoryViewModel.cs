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
        public ICollection<History> HT { get; set; } //History items
    }
}