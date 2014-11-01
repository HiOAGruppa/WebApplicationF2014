using System;
namespace BLL
{
    public interface ISalesItemBLL
    {
        bool addSalesItem(Model.SalesItem item);
        bool editSalesItem(Model.SalesItem item);
        Model.SalesItem findSalesItem(int id);
        System.Collections.Generic.List<Model.SalesItem> getAllSalesItems();
        System.Collections.Generic.List<Model.SalesItem> getSalesItemsWithGenre();
        bool removeSalesItem(Model.SalesItem item);
        System.Collections.Generic.List<Model.SalesItem> searchSalesItems(string nameQuery);
    }
}
