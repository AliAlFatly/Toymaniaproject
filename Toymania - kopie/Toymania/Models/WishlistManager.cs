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
    public class WishlistManager
    {
        TSE15 db = new TSE15();
        string WID { get; set; } //cart Id
        List<int> WIDLI { get; set; }       //wishlistidlist
        public const string WishlistSessionKey = "WishlistId";

        public int LastRecordW()
        {
            IQueryable<int> WILIQ = db.Wishlist.Select(x => x.WishlistId); //WishlistIdList IQueryable
            List<int> WL = new List<int> { };   //WishlistIdList
            foreach (int LOI in WILIQ)
            {
                WL.Add(LOI);
            }
            int WR = WL.Last();   //wishlistidresult
            return WR;
        }

        public static WishlistManager GW(HttpContextBase context) //get
        {
            var wl = new WishlistManager();
            wl.WID = wl.GEmail(context);
            return wl;
        }
        
        //helper method to simplify shopping cart calls
        public static WishlistManager GW(Controller controller)
        {
            return GW(controller.HttpContext);
        }

        public Wishlist RWT(Toy toy, int? id) //return wishlist's toys
        {
            if (toy != null)
            {
                var WT = db.Wishlist.SingleOrDefault(
                        c => c.Email == WID && c.ToysId == toy.ToysId);
                return WT;
            }
            else
            {
                var WT = db.Wishlist.SingleOrDefault(
                        c => c.Email == WID && c.ToysId == id);
                return WT;
            }
        }

        public bool ToyExist(Toy t, Wishlist w) //checks if exist returns true or false, wishlist is for the email check and toy for the toysid finder
        {
            var a = from B in db.Wishlist where B.ToysId == t.ToysId && B.Email == w.Email select B;        
            try
            {
                var b = a.First().ToysId;
                var c = a.First().Email;
                if (b != t.ToysId && c != w.Email)                      //als het niet bestaat dan returnt dit een error dus try-catch implementeren om de error te handelen
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return true;
            }


        }

        public void ATW(Toy toy, int? id)    //Add to wishlist
        {
            var WT = RWT(toy, id);
            bool a = ToyExist(toy, WT);
            if(a) // if exists dont add
            {
                WT = new Wishlist
                {
                    //WishlistId = LastRecordW() + 1,
                    Email = WID,
                    ToysId = toy.ToysId

                };
                db.Wishlist.Add(WT);
                db.SaveChanges();
            };

        }

        public void RFW(int id) //remove
        {
            var WT = db.Wishlist.Single(
                w => w.Email == WID &&
                        w.WishlistId == id);
            db.Wishlist.Remove(WT);
            db.SaveChanges();
        }

        public void EW() // empty
        {
            var WT = db.Wishlist.Where(w => w.Email == WID);
            foreach (var W in WT)
            {
                db.Wishlist.Remove(W);
            }
            db.SaveChanges();
        }

        public List<Wishlist> GWT() //get wishlist toy
        {
            return db.Wishlist.Where(
                w => w.Email == WID).ToList();
        }

        public string GEmail(HttpContextBase context) //get Email
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

        public bool TET(Wishlist w) //checks if exist returns true or false, wishlist is for the email check and toy for the toysid finder
        {
            try
            {
                var a = from B in db.Wishlist where B.ToysId == w.ToysId && B.Email == w.Email select B;
                var b = a.First().ToysId;
                var c = a.First().Email;
                if (b != w.ToysId && c != w.Email)                      //als het niet bestaat dan returnt dit een error dus try-catch implementeren om de error te handelen
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return true;
            }
        }

        public void MigrateWishlist(string Email)   //neemt temprarly session email(code) en vervangt het door de ingelogde email
        {
            var WLC = db.Wishlist.Where(
                w => w.Email == WID);               //toevoegen foreach that exist delete

            foreach (Wishlist I in WLC)
            {
                I.Email = Email;
            }
            db.SaveChanges();

            var a = GWT();
            foreach (Wishlist I in a)
            {
                if (TET(I))
                {
                    db.Wishlist.Remove(I);
                    db.SaveChanges();
                }
                
            }


            
        }
    }
}