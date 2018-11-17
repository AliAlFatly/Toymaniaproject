//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Toymania.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Toymania.Models;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class Toy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Toy()
        {
            this.Cart = new HashSet<Cart>();
            this.OrderDetails = new HashSet<OrderDetails>();
            this.Wishlist = new HashSet<Wishlist>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ToysId { get; set; }
        [DisplayName("Toy's name")]
        public string ToysName { get; set; }
        [DisplayName("Category")]
        public Nullable<int> CategoryId { get; set; }
        [DisplayName("Producer")]
        public Nullable<int> ProducerId { get; set; }
        [DisplayName("Store Picture")]
        public string ItemArtUrl { get; set; }
        [Required(ErrorMessage = "Price is required")]
        //[Range(typeof(decimal), "0.00" , "10000.00", ErrorMessage = "price Must be between 0.1 and 10000")]
        //[Range(typeof(decimal), , ErrorMessage = "price Must be between 0.1 and 10000")]
        [Range(0.01, 10000, ErrorMessage = "price Must be between 0.01 and 10000")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public Nullable<int> Counter { get; set; }
        public Nullable<int> MinimumAge { get; set; }
        [DisplayName("Subcategory")]
        public Nullable<int> SubCategoryId { get; set; }
        [DisplayName("Detail Picture")]
        public string ItemArtUrl2 { get; set; }
        [DisplayName("Detail Picture 2")]
        public string ItemArtUrl3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Cart { get; set; }
        public virtual Categories Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual Producers Producers { get; set; }
        public virtual SubCategories SubCategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wishlist> Wishlist { get; set; }
    }
}
