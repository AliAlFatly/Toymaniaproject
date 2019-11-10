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
    public class WishlistService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        string WishlistId { get; set; }
        List<int> WishlistIdList { get; set; }
        public const string WishlistSessionKey = "WishlistId";

        public int LastRecord()
        {
            if (db.Wishlist.Find(0) == null)
            {
                return 0;
            }
            else
            {
                IQueryable<int> WishlistIQueryable = db.Wishlist.Select(x => x.WishlistId);
                List<int> IdList = new List<int> { };
                foreach (int Id in WishlistIQueryable)
                {
                    IdList.Add(Id);
                }
                int lastRecord = IdList.Last();
                return lastRecord;
            }

        }

        public string GetWishlistId()
        {
            return this.WishlistId;
        }

        public string GetWishlistId(HttpContextBase context)
        {
            if (context.Session[WishlistSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[WishlistSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    Guid tempWEmail = Guid.NewGuid();
                    context.Session[WishlistSessionKey] = tempWEmail.ToString();

                }
            }
            return context.Session[WishlistSessionKey].ToString();
        }

        public static WishlistService GetWishlist(HttpContextBase context)
        {
            var Wishlist = new WishlistService();
            Wishlist.WishlistId = Wishlist.GetWishlistId(context);
            return Wishlist;
        }

        public static WishlistService GetWishlist(Controller controller)
        {
            return GetWishlist(controller.HttpContext);
        }

        public List<Wishlist> GetToys()
        {
            return db.Wishlist.Where(w => w.Email == WishlistId).ToList();
        }

        public void AddToWishlist(Toy toy, int? id)
        {
            var WishlistToys = db.Wishlist.SingleOrDefault(c => c.Email == WishlistId && c.ToysId == toy.ToysId);
            if (WishlistToys == null)
            {
                WishlistToys = new Wishlist
                {
                    //WishlistId = LastRecordW() + 1,
                    Email = WishlistId,
                    ToysId = toy.ToysId

                };
                db.Wishlist.Add(WishlistToys);
                db.SaveChanges();
            };

        }

        public void DeleteFromWishlist(int id)
        {
            var WishlistToy = db.Wishlist.Single(w => w.Email == WishlistId && w.WishlistId == id);
            db.Wishlist.Remove(WishlistToy);
            db.SaveChanges();
        }

        public void EmptyWishlist()
        {
            var WishlistToys = db.Wishlist.Where(w => w.Email == WishlistId);
            foreach (var Toy in WishlistToys)
            {
                db.Wishlist.Remove(Toy);
            }
            db.SaveChanges();
        }

        public List<Wishlist> GetWishList()
        {
            return db.Wishlist.Where(w => w.Email == WishlistId).ToList();
        }

        public void MigrateWishlist(string Email)
        {
            var Wishlist = db.Wishlist.Where(w => w.Email == WishlistId);

            foreach (Wishlist Record in Wishlist)
            {
                Record.Email = Email;
            }
            db.SaveChanges();
        }
    }
}