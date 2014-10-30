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
            db.addSalesItemInOrder(item);
        }

        public void addOrder(Order order)
        {
            db.addOrder(order);
        }

        public Order getOrder(int orderId)
        {
            return db.getOrder(orderId);
        }

        public Order getOrderWithItems(int orderId)
        {
            return db.getOrderWithItems(orderId);
        }

        public void removeOrder(int id)
        {
            db.removeOrder(id);
        }

        public List<Order> getOrders()
        {
            return db.getOrders();
        }


    }
}
