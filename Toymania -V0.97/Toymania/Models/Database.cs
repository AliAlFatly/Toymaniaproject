using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Toymania.Models
{
    //test
    public partial class Cart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        [ForeignKey("Toy")]
        public int ToyId { get; set; }
        public int Count { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }

        public virtual Toy Toy { get; set; }
    }

    public partial class Categories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categories()
        {
            this.SubCategories = new HashSet<SubCategories>();
            this.Toy = new HashSet<Toy>();
        }

        [ScaffoldColumn(false)]
        [DisplayName("Category")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [DisplayName("Category")]
        public string CName { get; set; }
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubCategories> SubCategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Toy> Toy { get; set; }
    }

    public partial class Coupon
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        [Range(0.01, 1000, ErrorMessage = "price Must be between 0.01 and 1000")]
        public decimal Value { get; set; }
        public bool Used { get; set; }
    }

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetails>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }
        [ScaffoldColumn(false)]
        public string Username { get; set; }
        //[StringLength(160, MinimumLength = 1, ErrorMessage = "Uw eerste naam moet minimaal 1 en maximaal 160 character bevatten")]
        //[Required(ErrorMessage ="Please entere your firstname")]
        public string FirstName { get; set; }
        //[Required(ErrorMessage = "Please entere your lastname")]
        public string LastName { get; set; }
        //[Required(ErrorMessage = "Please entere your address")]
        public string Address { get; set; }
        //[Required(ErrorMessage = "Please entere your city")]
        public string City { get; set; }
        //[Required(ErrorMessage = "Please entere your state")]
        public string State { get; set; }
        //[Required(ErrorMessage = "Please entere your postalcode")]
        public string PostalCode { get; set; }
        //[Required(ErrorMessage = "Please entere your country")]
        public string Country { get; set; }
        //[ScaffoldColumn(false)]
        //public string Email { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<decimal> Total { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> OrderDate { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<int> year { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<int> month { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<int> day { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<int> minute { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<int> second { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<int> hour { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<int> week { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }

    public partial class OrderDetails
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }
        public Nullable<int> OrderId { get; set; }
        [ForeignKey("Toy")]
        public Nullable<int> ToyId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string Status { get; set; }
        [ScaffoldColumn(false)]
        public string CName { get; set; }
        [ScaffoldColumn(false)]
        public string SCName { get; set; }
        public Nullable<int> Week { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> year { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> Hour { get; set; }
        public Nullable<int> Minute { get; set; }
        public string tracker { get; set; }

        public virtual Order Order { get; set; }
        public virtual Toy Toy { get; set; }
    }

    public partial class Producers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Producers()
        {
            this.Toy = new HashSet<Toy>();
        }

        [ScaffoldColumn(false)]
        [DisplayName("Producer")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProducerId { get; set; }
        [DisplayName("Producer")]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Toy> Toy { get; set; }
    }

    public partial class SubCategories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubCategories()
        {
            this.Toy = new HashSet<Toy>();
        }

        public int SubCategoryId { get; set; }
        [DisplayName("Subcategory")]
        public string SCName { get; set; }
        public string Description { get; set; }
        public Nullable<int> CategoryId { get; set; }

        public virtual Categories Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Toy> Toy { get; set; }
    }

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
        [Required(ErrorMessage ="Please entere the toy's name")]
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
        [Range(1, 100, ErrorMessage = "minimum age must be between 1 and 100")]
        [Required(ErrorMessage = "Please enter the minimum age")]
        public Nullable<int> MinimumAge { get; set; }
        [DisplayName("Subcategory")]
        public Nullable<int> SubCategoryId { get; set; }
        [DisplayName("Detail Picture")]
        public string ItemArtUrl2 { get; set; }
        [DisplayName("Archived Item")]
        public string Archive { get; set; }

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

    public partial class Wishlist
    {
        [ScaffoldColumn(false)]
        public int WishlistId { get; set; }
        public string Email { get; set; }
        public Nullable<int> ToysId { get; set; }

        public virtual Toy Toy { get; set; }
    }



}