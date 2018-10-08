using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Toymania.Models
{
    public class TSD7 : DbContext
    {
        public virtual DbSet<Toy> Toy { get; set; }
        public virtual DbSet<Producers> Producers { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }



    }
}