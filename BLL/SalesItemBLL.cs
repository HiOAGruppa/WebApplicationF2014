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
            db.SalesItems.Add(item);
            db.SaveChanges();
        }

        public void removeSalesItem(SalesItem item)
        {
            db.SalesItems.Remove(item);
            db.SaveChanges();
        }

        public void editSalesItem(SalesItem item)
        {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public List<SalesItem> getSalesItemsWithGenre()
        {
            return db.SalesItems.Include(a => a.Genre).ToList();
        }
    }
}
