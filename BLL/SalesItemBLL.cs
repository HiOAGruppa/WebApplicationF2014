using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;
using System.Data.Entity;

namespace BLL
{
    public class SalesItemBLL
    {
        StoreContext db;
        public SalesItemBLL() {
            db = new StoreContext();
        }

        public List<SalesItem> getAllSalesItems()
        {
            return db.getAllSalesItems();
        }

        public List<SalesItem> searchSalesItems(String nameQuery)
        {
            return db.searchSalesItems(nameQuery);
        }

        public SalesItem findSalesItem(int id)
        {
            return db.SalesItems.Find(id);
        }

        public void addSalesItem(SalesItem item)
        {
            db.addSalesItem(item);
        }

        public void removeSalesItem(SalesItem item)
        {
            db.removeSalesItem(item);
        }

        public void editSalesItem(SalesItem item)
        {
            db.editSalesItem(item);
        }

        public List<SalesItem> getSalesItemsWithGenre()
        {
            return db.getSalesItemsWithGenre();
        }
    }
}
