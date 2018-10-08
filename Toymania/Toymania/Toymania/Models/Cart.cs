namespace Toymania.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public Nullable<int> ToyId { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }

        public virtual Toy Toy { get; set; }
    }
}
