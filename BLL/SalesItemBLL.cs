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
    public class SalesItemBLL : BLL.ISalesItemBLL
    {
        StoreContext db;

        private IStoreManagerRepository _repository;

        public SalesItemBLL() {
            db = new StoreContext();
            _repository = new StoreManagerRepository();
        }
        public SalesItemBLL(IStoreManagerRepository stub)
        {
            db = new StoreContext();
            _repository = stub;
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
            _repository.addSalesItem(item);
        }

        public void removeSalesItem(SalesItem item)
        {
            _repository.removeSalesItem(item);
        }

        public void editSalesItem(SalesItem item)
        {
            _repository.editSalesItem(item);
        }

        public List<SalesItem> getSalesItemsWithGenre()
        {
            return _repository.getSalesItemsWithGenre();
        }
    }
}
