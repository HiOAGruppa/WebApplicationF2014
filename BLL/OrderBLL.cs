using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

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


    }
}
