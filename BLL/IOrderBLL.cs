using System;
namespace BLL
{
    public interface IOrderBLL
    {
        void addOrder(Model.Order order);
        void addSalesItemInOrder(Model.OrderSalesItem item);
        Model.Order getOrder(int orderId);
        System.Collections.Generic.List<Model.Order> getOrders();
        Model.Order getOrderWithItems(int orderId);
        System.Collections.Generic.List<Model.Order> getUserOrders(int userId);
        bool removeOrder(int id);
    }
}
