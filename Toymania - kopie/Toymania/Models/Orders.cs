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
    public class Orders
    {
        TSE15 db = new TSE15();


        public int LastRecordO()
        {
            IQueryable<int> OILIQ = db.Order.Select(x => x.OrderId); //OrderIdList IQueryable
            List<int> OL = new List<int> { };   //OrderList
            foreach (int LOI in OILIQ){OL.Add(LOI);}
            int LO = OL.Last();
            return LO;
        }

        public int LastRecordOD()
        {
            IQueryable<int> ODILIQ = db.OrderDetails.Select(x => x.OrderDetailId); //OrderDetailList IQueryable
            List<int> ODL = new List<int> { };   //OrderDetailList
            foreach (int LOI in ODILIQ) { ODL.Add(LOI); }
            int LOD = ODL.Last();
            return LOD;
        }




        public List<Order> GOS(HttpContextBase c, string S) //get Orders
        {
            var Order = new Order();
            IQueryable<Order> OQ = from o in db.Order
                                   where o.Username == c.User.Identity.Name
                                   select (Order)o;
            List<Order> OL = new List<Order>();   //HistoryIdList
            foreach (Order O in OQ) { OL.Add(O); }
            List<Order> FOL = new List<Order>();
            foreach (var I in OL)
            {
                if (I.OrderDetails != null)
                {
                    var OD = new List<OrderDetails>(I.OrderDetails);
                    foreach (var J in OD)
                    {
                        if (FOL.Find(x => x.OrderId == I.OrderId) == null && J.Status == S)
                        {
                            FOL.Add(I);
                        }
                    }
                }

            }
            return FOL;
        }

        public ICollection<OrderDetails> GDS(int id, string S) //get OrdersDetails
        {
            var OD = new OrderDetails();
            var ODQ = from o in db.OrderDetails
                      where o.Order.OrderId == id && o.Status == S
                      select o;
            List<OrderDetails> OL = new List<OrderDetails> { };
            foreach (OrderDetails I in ODQ) { OL.Add(I); }
            return OL;
        }

        public List<Order> GO(HttpContextBase c) //get Orders
        {
            var Order = new Order();
            IQueryable<Order> OQ = from o in db.Order
                    where o.Username == c.User.Identity.Name
                    select (Order)o;
            List<Order> OL = new List<Order> { };   //HistoryIdList
            foreach (Order O in OQ) { OL.Add(O); }
            return OL;
        }

        public ICollection<OrderDetails> GD(int id) //get OrdersDetails
        {
            var OD = new OrderDetails();
            var ODQ = from o in db.OrderDetails
                      where o.Order.OrderId == id
                      select o;
            List<OrderDetails> OL = new List<OrderDetails> { };
            foreach (OrderDetails I in ODQ) { OL.Add(I); }
            return OL;
        }



        public void OToH(Order O, HttpContextBase c) // order to history
        {      
            if(O != null && c != null)
            {
                //history decided by the orderdetails status(
                Order CO = O;
                IQueryable<ICollection<OrderDetails>> OD = from od in db.Order
                                                           where od.OrderId == O.OrderId
                                                           select od.OrderDetails;
                ICollection<OrderDetails> ODIC = OD.First();


                //History H = new History
                //{
                //    Email = c.User.Identity.Name,
                //    OrderDetails = ODIC
                //};

                //db.History.Add(H);

                foreach (OrderDetails ODT in ODIC)
                {                   
                    //ODT.OrderId = null;
                    //ODT.HistoryId = H.HistoryId;
                    ODT.Status = "Completed";
                    db.SaveChanges();
                }

                //UserHistory UH = new UserHistory();
                //UH.ATH(H);                
                RFO(CO.OrderId);
            }
        }

        public void AOD(Order O) //add order detail
        {
            if (O != null)
            {
                Order CO = O;
                ICollection<OrderDetails> ODIC = CO.OrderDetails;
                foreach(OrderDetails OD in ODIC)
                {
                    db.OrderDetails.Add(OD);
                }
            }
        }

        public void ATO(Order O, HttpContextBase c)    //Add to Orders
        {
            if (O != null && c.User.Identity.Name != null)
            {
                Order o = new Order
                {
                    //OrderId = LastRecordO() + 1,
                    Username = c.User.Identity.Name,
                    FirstName = O.FirstName,
                    LastName = O.LastName,
                    Address = O.Address,
                    year = O.year,
                    month = O.month,
                    day = O.day,
                    
                    minute = O.minute,
                    second = O.second,
                    hour = O.hour,
                    week = O.week,
                    
                    City = O.City,
                    State = O.State,
                    PostalCode = O.PostalCode,
                    Country = O.Country,
                    //Email = c.User.Identity.Name,
                    Total = O.Total,
                    OrderDate = O.OrderDate,
                    //OrderDetails = O.OrderDetails

                };
                db.Order.Add(o);    //de net gemaakt order wordt aan de db toegevoegd
                db.SaveChanges();   //db update
                //ATOD(o);

            }
        }

        public void RFO(int id) //Remove from Orders
        {
            var Order = db.Order.Single(o => o.OrderId == id);
            if (Order != null)
            {
                db.Order.Remove(Order);
                db.SaveChanges();
            }
        }


    }
}