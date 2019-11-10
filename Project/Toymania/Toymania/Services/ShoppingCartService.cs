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

namespace Toymania.Services
{
    public class ShoppingCartService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        string CartId { get; set; } //cart Id
        List<int> RecordIdList { get; set; }
        public const string CartSessionKey = "CartId";

        public static ShoppingCartService GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCartService();
            cart.CartId = cart.GetCartId(context);
            return cart;
        }

        public string GetCartId()
        {
            return this.CartId;
        }

        public string GetCartId(HttpContextBase context)
        {
            //if no CartSessionKey exist, generate then return generated one else just return current CartSessionKey
            if (context.Session[CartSessionKey] == null)
            {
                //if user is logged set CartSessionKey to user name (which is unique), NOTE Identity.Name = Email
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                //else generate random CartSessionKey and store it as session and on user side as cookie, when browser is shutdown, cookie is set to delete???
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();

                }
            }
            //return current/generated/set CartSessionId
            return context.Session[CartSessionKey].ToString();
        }

        public int GetLastRecord()
        {
            if (db.Cart.Find(0) == null)
            {
                return 0;
            }
            else
            {
                var RecordIQueryable = db.Cart.Select(x => x.RecordId);
                List<int> RecordList = new List<int> { };
                foreach (int Id in RecordIQueryable)
                {
                    RecordList.Add(Id);
                }
                int LastRecord = RecordList.Last();
                return LastRecord;
            }
        }

        public static ShoppingCartService GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Toy toy)
        {

            var CartToy = db.Cart.SingleOrDefault(
                        c => c.CartId == CartId && c.ToyId == toy.ToysId);

            if (CartToy == null)
            {
                CartToy = new Cart
                {
                    RecordId = GetLastRecord() + 1, //0,
                    ToyId = toy.ToysId,
                    CartId = CartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.Cart.Add(CartToy);

            }
            else
            {
                CartToy.Count++;
            }
            db.SaveChanges();
        }

        public int RemoveFromCart(int id) 
        {
            var CartToy = db.Cart.Single(
                cart => cart.CartId == CartId &&
                        cart.RecordId == id);
            int ToyCount = 0;
            if (CartToy != null)
            {
                if (CartToy.Count > 1)
                {
                    CartToy.Count--;
                    ToyCount = CartToy.Count;
                }
                else
                {
                    db.Cart.Remove(CartToy);
                }
                db.SaveChanges();
            }
            return ToyCount;
        }

        public void EmptyCart()
        {
            var CartToy = db.Cart.Where(cart => cart.CartId == CartId);
            foreach (var Toy in CartToy)
            {
                db.Cart.Remove(Toy);
            }
            db.SaveChanges();
        }

        public List<Cart> GetCartToys()
        {
            return db.Cart.Where(
                c => c.CartId == CartId).ToList();
        }

        public int GetCount()
        {
            int? count = (from CartToy in db.Cart
                          where CartToy.CartId == CartId
                          select (int?)CartToy.Count).Sum();
            return count ?? 0;
        }

        public decimal GetTotalPrice()
        {
            decimal? total = (from CartToy in db.Cart
                              where CartToy.CartId == CartId
                              select (int?)CartToy.Count * CartToy.Toy.Price).Sum();
            return total ?? decimal.Zero;
        }

        public decimal GetTotalPrice(HttpContextBase context)
        {
            decimal? total = (from CartToy in db.Cart
                              where CartToy.CartId == context.User.Identity.Name
                              select (int?)CartToy.Count * CartToy.Toy.Price).Sum();
            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order, HttpContextBase context)
        {
            decimal orderTotal = 0;
            var CartToy = GetCartToys();
            OrdersService Order = new OrdersService();
            //List<OrderDetails> ODL = new List<OrderDetails> { };
            foreach (var Toy in CartToy)
            {
                double w = DateTime.Now.DayOfYear / 7;
                var week = (int)Math.Ceiling(w);

                var OrderDetail = new OrderDetails           //alles wat in de cart zit wordt toegevoegd als orderdetails
                {
                    //OrderDetailId = O.LastRecordOD() + 1,
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
                    CategoryName = Toy.Toy.Categories.CategoryName,
                    SubCategoryName = Toy.Toy.SubCategories.SubCategoryName


                };
                order.Total += (Toy.Count * Toy.Toy.Price);     //per toy wordt de orderprijs bijgewerkt
                db.OrderDetails.Add(OrderDetail);                        //
            }

            Order.AddOrder(order, context);      //Add order to db
            order.Total = orderTotal;       //Put total to 0
            db.SaveChanges();               //db save
            EmptyCart();                    //empty cart
            return order.OrderId;           //return id
        }

        //MigrateCart when user logs in, set CartId foreach item in the cart to the users Email.
        public void MigrateCart(string Email)
        {
            var shoppingCart = db.Cart.Where(c => c.CartId == CartId);
            foreach (Cart I in shoppingCart)
            {
                I.CartId = Email;
            }
            db.SaveChanges();
        }



    }
}