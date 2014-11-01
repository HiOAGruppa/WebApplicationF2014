using System;
namespace BLL
{
    public interface ISalesItemBLL
    {
        void addSalesItem(Model.SalesItem item);
        void editSalesItem(Model.SalesItem item);
        Model.SalesItem findSalesItem(int id);
        System.Collections.Generic.List<Model.SalesItem> getAllSalesItems();
        System.Collections.Generic.List<Model.SalesItem> getSalesItemsWithGenre();
        void removeSalesItem(Model.SalesItem item);
        System.Collections.Generic.List<Model.SalesItem> searchSalesItems(string nameQuery);
    }
}
