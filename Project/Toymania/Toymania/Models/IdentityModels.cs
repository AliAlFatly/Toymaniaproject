using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Toymania.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [ScaffoldColumn(false)]
        public decimal balance { get; set; }
        [ScaffoldColumn(false)]
        public string Role { get; set; }

        //public DbSet<ToyT> t { get; set; }

        //DbSet<CartT> Cart { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("TSE", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Toy>().HasKey(p => p.ToysId);
            modelBuilder.Entity<Cart>().HasKey(p => p.RecordId);
            modelBuilder.Entity<Categories>().HasKey(p => p.CategoryId);
            modelBuilder.Entity<SubCategories>().HasKey(p => p.SubCategoryId);
            modelBuilder.Entity<Coupon>().HasKey(p => p.Id);
            modelBuilder.Entity<Order>().HasKey(p => p.OrderId);
            modelBuilder.Entity<OrderDetails>().HasKey(p => p.OrderDetailId);
            modelBuilder.Entity<Producers>().HasKey(p => p.ProducerId);
            modelBuilder.Entity<Wishlist>().HasKey(p => p.WishlistId);





        }

        public System.Data.Entity.DbSet<Toymania.Models.Toy> Toy { get; set; }
        public System.Data.Entity.DbSet<Toymania.Models.Cart> Cart { get; set; }
        public System.Data.Entity.DbSet<Toymania.Models.Categories> Categories { get; set; }
        public System.Data.Entity.DbSet<Toymania.Models.SubCategories> SubCategories { get; set; }
        public System.Data.Entity.DbSet<Toymania.Models.Coupon> Coupon { get; set; }
        public System.Data.Entity.DbSet<Toymania.Models.Order> Order { get; set; }
        public System.Data.Entity.DbSet<Toymania.Models.OrderDetails> OrderDetails { get; set; }
        public System.Data.Entity.DbSet<Toymania.Models.Producers> Producers { get; set; }
        public System.Data.Entity.DbSet<Toymania.Models.Wishlist> Wishlist { get; set; }

        //public System.Data.Entity.DbSet<Toymania.Models.ApplicationUser> ApplicationUsers { get; set; }
    }

}
