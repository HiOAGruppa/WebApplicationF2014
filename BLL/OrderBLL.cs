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
    public class OrderBLL : BLL.IOrderBLL
    {
        StoreContext db;
        bool test = false;

        private IStoreManagerRepository _repository;

        public OrderBLL() {
            db = new StoreContext();
            _repository = new StoreManagerRepository();
        }
        public OrderBLL(IStoreManagerRepository stub)
        {
            test = true;
            db = new StoreContext();
            _repository = stub;
        }
        
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
            if(test)
                return _repository.getOrders();
            else
                return db.getOrders();
        }


    }
}
