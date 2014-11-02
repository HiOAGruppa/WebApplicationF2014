using System;
namespace DAL
{
    public interface IStoreManagerRepository
    {
        bool addSalesItem(Model.SalesItem item);
        bool editSalesItem(Model.SalesItem item);
        bool editUser(int userId, Model.User user);
        System.Collections.Generic.List<Model.Order> getOrders();
        System.Collections.Generic.List<Model.SalesItem> getSalesItemsWithGenre();
        System.Collections.Generic.List<Model.User> getUsers();
        Model.User getUser(int id);
        bool removeSalesItem(Model.SalesItem item);
        bool removeUser(Model.User user);
        bool removeOrder(int id);
        Model.SalesItem findSalesItem(int id);
    }
}
