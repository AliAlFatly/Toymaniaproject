namespace Toymania.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Toy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Toy()
        {
            this.Cart = new HashSet<Cart>();
            this.OrderDetails = new HashSet<OrderDetails>();
        }
        [Key]
        public int ToysId { get; set; }
        public string ToysName { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> ProducerId { get; set; }
        public string ItemArtUrl { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Counter { get; set; }
        public Nullable<int> MinimumAge { get; set; }
        public string type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Cart { get; set; }
        public virtual Categories Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual Producers Producers { get; set; }
    }
}