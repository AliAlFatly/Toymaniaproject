using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Toymania.Models;

namespace Toymania.Models
{
    //GC = get cart, RFC = remove from cart, ATC = add to cart, CT = cart toy(toy in cart), EC = EMPTY the cart, GCI = get cart id, GT = get total, CO = create order, 

    public class ShoppingCart
    {

        ApplicationDbContext db = new ApplicationDbContext();
        string SCID { get; set; } //cart Id
        List<int> RIDLI { get; set; }
        public const string CartSessionKey = "CartId";

        public int LastRecord()
        {
            //if (db.Cart.Select(x => x.RecordId) != null)
            //{
            if (db.Cart.Find(0) == null)
            {
                return 0;
            }
            else
            {
                var RILIQ = db.Cart.Select(x => x.RecordId);
                List<int> RL = new List<int> { };
                foreach (int LOI in RILIQ)
                {
                    RL.Add(LOI);
                }
                int LR = RL.Last();
                return LR;
            }
        }

        public static ShoppingCart GC(HttpContextBase context) //get
        {
            var cart = new ShoppingCart();
            cart.SCID = cart.GCI(context);
                return cart;
        }
        //helper method to simplify shopping cart calls
        public static ShoppingCart GC(Controller controller)
        {
            return GC(controller.HttpContext);
        }

        public Cart RCT (Toy toy, int? id)
        {
            if (toy != null)
            {
                var CT = db.Cart.SingleOrDefault(
                        c => c.CartId == SCID && c.ToyId == toy.ToysId);
                return CT;
            }
            else
            {
                var CT = db.Cart.SingleOrDefault(
                        c => c.CartId == SCID && c.ToyId == id);
                return CT;
            }

        }

        public void ATC(Toy toy, int? id)    //Add to cart
        {

            var CT = RCT(toy, id);

            //var CT = db.Cart.SingleOrDefault(
            //    c => c.CartId == SCID && c.ToyId == toy.ToysId);
            if (CT == null)
            {
                CT = new Cart
                {
                    RecordId = LastRecord() + 1, //0,
                    ToyId = toy.ToysId,
                    CartId = SCID,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.Cart.Add(CT);

            }
            else
            {
                CT.Count++;
            }
            db.SaveChanges();
        }

        public int RFC(int id) //remove
        {
            var CT = db.Cart.Single(
                cart => cart.CartId == SCID && 
                        cart.RecordId == id);

            int ToyCount = 0;

            if (CT != null)
            {
                if (CT.Count > 1)
                {
                    CT.Count--;
                    ToyCount = CT.Count;

                }
                else
                {
                    db.Cart.Remove(CT);
                }
                db.SaveChanges();
            }
            return ToyCount;
        }

        public void EC() // empty
        {
            var CT = db.Cart.Where(cart => cart.CartId == SCID);
            foreach (var C in CT)
            {
                db.Cart.Remove(C);
            }
            db.SaveChanges();
        }

        public List<Cart> GCT() //get cart toy
        {
            return db.Cart.Where(
                c => c.CartId == SCID).ToList();
        }

        public List<Cart> GCTT(HttpContextBase context) //get cart toy test                   aaaaaaaaaaaaaaaaaaaa
        {
            //var userId = IdentityResult.               
        
            if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
            {
                context.Session[CartSessionKey] = context.User.Identity.Name;

                return db.Cart.Where(
                    c => c.CartId == context.User.Identity.Name).ToList();
            }
            else
            {
                return db.Cart.Where(
                    c => c.CartId == SCID).ToList();
            }

        }

        public int GCount() //get count
        {
            int? count = (from CT in db.Cart
                          where CT.CartId == SCID
                          select (int?)CT.Count).Sum();
            return count ?? 0;
        }



        public decimal GT() //get total
        {
            decimal? total = (from CT in db.Cart
                              where CT.CartId == SCID
                              select (int?)CT.Count* CT.Toy.Price).Sum();
            return total ?? decimal.Zero;
        }

        public decimal GTH(HttpContextBase c) //get total
        {
            decimal? total = (from CT in db.Cart
                              where CT.CartId == c.User.Identity.Name
                              select (int?)CT.Count * CT.Toy.Price).Sum();
            return total ?? decimal.Zero;
        }

        public int CO(Order order, HttpContextBase c) // create order
        {
            decimal orderTotal = 0;
            var CT = GCT();
            Orders O = new Orders();
            //List<OrderDetails> ODL = new List<OrderDetails> { };

            foreach (var Toy in CT)                 
            {
                double w = DateTime.Now.DayOfYear / 7;
                var week = (int)Math.Ceiling(w);

                var OD = new OrderDetails           //alles wat in de cart zit wordt toegevoegd als orderdetails
                {
                    OrderDetailId = O.LastRecordOD() + 1,
                    ToyId = Toy.ToyId,
                    OrderId = order.OrderId,
                    UnitPrice = Toy.Toy.Price,
                    Quantity = Toy.Count,
                    Week = week,
                    Month = DateTime.Now.Month,
                    year = DateTime.Now.Year,
                    Day = DateTime.Now.Day,
                    Hour = DateTime.Now.Hour,
                    Minute = DateTime.Now.Minute,
                    
                    Status = "In Progress",
                    CName = Toy.Toy.Categories.CName,
                    SCName = Toy.Toy.SubCategories.SCName
                    
                    
                };

                order.Total += (Toy.Count * Toy.Toy.Price);     //per toy wordt de orderprijs bijgewerkt
                //ODL.Add(OD);                                    //orderdetail wordt aan de list toegevoegd 
                db.OrderDetails.Add(OD);                        //
            }

            O.ATO(order, c); // de order wordt toegoegd in de db
            order.Total = orderTotal;       //de totaal wordt 0
            db.SaveChanges();               //db save
            EC();                           //empty cart
            return order.OrderId;           //return id
        }

        public string GCI(HttpContextBase context) //get cart id
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();

                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string Email)
        {
            
            var shoppingCart = db.Cart.Where(
                c => c.CartId == SCID);

            
            foreach (Cart I in shoppingCart)
            {
                I.CartId = Email;
            }
            db.SaveChanges();
        }




    }
}