using System;
namespace DAL
{
    public interface IStoreManagerRepository
    {
        bool addSalesItem(Model.SalesItem item);
        bool editSalesItem(Model.SalesItem item);
        void editUser(int userId, Model.User user);
        System.Collections.Generic.List<Model.Order> getOrders();
        System.Collections.Generic.List<Model.SalesItem> getSalesItemsWithGenre();
        System.Collections.Generic.List<Model.User> getUsers();
        bool removeSalesItem(Model.SalesItem item);
        void removeUser(Model.User user);
        Model.SalesItem findSalesItem(int id);
    }
}
