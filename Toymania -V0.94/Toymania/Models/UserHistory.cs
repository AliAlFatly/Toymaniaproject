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
    public class UserHistory
    {
        ApplicationDbContext db = new ApplicationDbContext();
        List<int> HIDLI { get; set; }

        //public ICollection<History> GID(HttpContextBase context)
        //{
        //    var HistoryId = from h in db.History
        //                    where h.Email == context.User.Identity.GetUserName()
        //                    select (ICollection<History>)h;
        //    ICollection<History> HIDC = HistoryId.First();

        //    return HIDC;
        //}



        //public int LastRecord()
        //{

        //    IQueryable<int> HILIQ = db.History.Select(x => x.HistoryId); //HistoryIdList IQueryable
        //    List<int> HL = new List<int> { };   //HistoryIdList
        //    foreach (int LOI in HILIQ){HL.Add(LOI);}
        //    int LH = HL.Last();
        //    return LH;
        //}


        //public void ATH(History h)
        //{
        //    var HT = new History
        //    {
        //        HistoryId = h.HistoryId,
        //        Email = h.Email,
        //    };
        //    db.History.Add(HT);
        //    db.SaveChanges();
        //}


        //public ICollection<History> GH(HttpContextBase c)
        //{
        //    var HIQ = from h in db.History
        //                    where h.Email == c.User.Identity.GetUserName()
        //                    select (ICollection<History>)h;
        //    ICollection<History> H = HIQ.First();

        //    return H;
        //}

    }
}