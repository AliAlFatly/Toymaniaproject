using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Toymania.Models
{
    //GC = get cart, RFC = remove from cart, ATC = add to cart, CT = cart toy(toy in cart), EC = EMPTY the cart, GCI = get cart id, GT = get total, CO = create order, 






    public class ShoppingCart
    {
        TSE7 db = new TSE7();
        string SCID { get; set; }
        List<int> RIDLI { get; set; }
        public const string CartSessionKey = "CartId";

        public int LastRecord()
        {
            IQueryable<int> RILIQ = db.Cart.Select(x => x.RecordId); //recordid list IQueryable
            List<int> RL = new List<int> { };   //record list
            foreach (int LOI in RILIQ)
            {
                RL.Add(LOI);
            }
            //for (int i = 0; i < db.Cart.Single(x=>x.RecordId), i++ ){}

            //var RIL = db.Cart.ToLookup(e => e.RecordId);


            int LR = RL.Last();
            return LR;
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

        public void ATC(Toy toy)    //Add to cart
        {
            var CT = db.Cart.SingleOrDefault(
                c => c.CartId == SCID && c.ToyId == toy.ToysId);
            if (CT == null)
            {
                CT = new Cart
                {
                    RecordId = LastRecord() + 1,
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

        public int? RFC(int? id) //remove
        {
            var CT = db.Cart.Single(
                cart => cart.CartId == SCID && 
                        cart.RecordId == id);

            int? ToyCount = 0;

            if (CT == null)
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

        public int CO(Order order) // create order
        {
            decimal orderTotal = 0;
            var CT = GCT();
            foreach (var Toy in CT)
            {
                var OD = new OrderDetails
                {
                    ToyId = Toy.ToyId,
                    OrderId = order.OrderId,
                    UnitPrice = Toy.Toy.Price,
                    Quantity = Toy.Count,
                };

                order.Total += (Toy.Count * Toy.Toy.Price);

                db.OrderDetails.Add(OD);
            }
            order.Total = orderTotal;
            db.SaveChanges();
            EC();
            return order.OrderId;
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