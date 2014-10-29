using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.Entity;
using System.Diagnostics;

namespace BLL
{
    public class OrderBLL
    {
        StoreContext db = new StoreContext();
        public List<Model.Order> getUserOrders(int userId)
        {
            return db.getUserOrders(userId);
        }

        public void addSalesItemInOrder(OrderSalesItem item)
        {
            db.SalesItemInOrder.Add(item);
            db.SaveChanges();
        }

        public void addOrder(Order order)
        {
            db.addOrder(order);
        }

        public Order getOrder(int orderId)
        {
            return db.Orders.Where(a => a.OrderId == orderId).FirstOrDefault();
        }

        public Order getOrderWithItems(int orderId)
        {
            Order order = db.Orders.Include("SalesItems").ToList().Single(a => a.OrderId == orderId);
            Debug.WriteLine(order.SalesItems.ToString());
            return order;
        }

        public void removeOrder(int id)
        {
            Order order = getOrder(id);
            //7Debug.Print("order: " + order.OrderId + " " + order.UserId);
            db.Orders.Remove(order);
            db.SaveChanges();
        }

        public List<Order> getOrders()
        {
            var orders = db.Orders.Include(s => s.SalesItems.Select(i => i.SalesItem)).Include("ownerUser").ToList();
            return orders;
        }


    }
}
