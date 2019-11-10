using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toymania.Models;
using Toymania.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Toymania.Controllers;

namespace Toymania.Services
{
    public class OrdersService
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public int LastRecord()
        {
            if (db.Order.Find(0) == null)
            {
                return 0;
            }
            else
            {
                IQueryable<int> OrdersIQueryable = db.Order.Select(x => x.OrderId); //OrderIdList IQueryable
                List<int> OrderIdList = new List<int> { };   //OrderList
                foreach (int Id in OrdersIQueryable) { OrderIdList.Add(Id); }
                int LastOrderId = OrderIdList.Last();
                return LastOrderId + 1;
            }

        }

        public int LastOrderDetailRecord()
        {
            if (db.OrderDetails.Find(0) == null)
            {
                return 0;
            }
            else
            {
                IQueryable<int> OrderDetailsIQueryable = db.OrderDetails.Select(x => x.OrderDetailId); //OrderDetailList IQueryable
                List<int> OrderDetailIdList = new List<int> { };   //OrderDetailList
                foreach (int Id in OrderDetailsIQueryable) { OrderDetailIdList.Add(Id); }
                int LastOrderDetailId = OrderDetailIdList.Last();
                return LastOrderDetailId + 1;
            }
        }

        public List<Order> GetOrders(HttpContextBase context, string status) //get Orders based on status
        {
            var Order = new Order();
            var Orders = (from OrdersTable in db.Order
                                   where OrdersTable.Username == context.User.Identity.Name
                                   select (Order)OrdersTable).ToList();
            List<Order> OrdersList = new List<Order>();
            foreach (var order in Orders)
            {
                if (order.OrderDetails != null)
                {
                    var OrderDetail = new List<OrderDetails>(order.OrderDetails);
                    foreach (var orderDetail in OrderDetail)
                    {
                        if (OrdersList.Find(x => x.OrderId == order.OrderId) == null && orderDetail.Status == status)
                        {
                            OrdersList.Add(order);
                        }
                    }
                }

            }
            return OrdersList;
        }

        public ICollection<OrderDetails> GetOrderDetails(int id, string Status)
        {
            var OrderDetails = (from OrderDetailsTable in db.OrderDetails
                      where OrderDetailsTable.Order.OrderId == id && OrderDetailsTable.Status == Status
                      select OrderDetailsTable).ToList();

            return OrderDetails;
        }

        public ICollection<OrderDetails> GetOrderDetails(int id) //get OrdersDetails
        {
            var OrderDetails = (from o in db.OrderDetails
                      where o.Order.OrderId == id
                      select o).ToList();
            return OrderDetails;
        }

        public List<Order> GetOrders(HttpContextBase context)
        {
            var Orders = (from OrdersTable in db.Order
                                   where OrdersTable.Username == context.User.Identity.Name
                                   select (Order)OrdersTable).ToList();
            return Orders;
        }

        public ICollection<OrderDetails> GD(int id)
        {
            var OrderDetails = (from OrderDetailsTable in db.OrderDetails
                      where OrderDetailsTable.Order.OrderId == id
                      select OrderDetailsTable).ToList();
            return OrderDetails;
        }

        public void OrderToHistory(Order order, HttpContextBase context)
        {
            if (order != null && context != null)
            {
                //Status define history
                Order Order = order;
                var OrderDetails = (from OrdersTable in db.Order
                                        where OrdersTable.OrderId == Order.OrderId
                                        select OrdersTable.OrderDetails).First();

                foreach (OrderDetails OrderDetail in OrderDetails)
                {
                    OrderDetail.Status = "Completed";
                    db.SaveChanges();
                }
                RemoveFromOrders(Order.OrderId);
            }
        }

        public void AddOrderDetail(Order order)
        {
            if (order != null)
            {
                Order Order = order;
                ICollection<OrderDetails> OrderDetailsList = Order.OrderDetails;
                foreach (OrderDetails OrderDetails in OrderDetailsList)
                {
                    db.OrderDetails.Add(OrderDetails);
                }
                db.SaveChanges();
            }
        }

        public void AddOrder(Order order, HttpContextBase context)
        {
            if (order != null && context.User.Identity.Name != null)
            {
                Order Order = order;
                Order.Username = context.User.Identity.Name;
                db.Order.Add(Order);
                db.SaveChanges();
                AddOrderDetail(Order);
            }
        }


        public void RemoveFromOrders(int id)
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