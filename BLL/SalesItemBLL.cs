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
        bool test;

        public SalesItemBLL() {
            db = new StoreContext();
            _repository = new StoreManagerRepository();
        }
        public SalesItemBLL(IStoreManagerRepository stub)
        {
            test = true;
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
            if (test)
                return _repository.findSalesItem(id);
            else
                return db.findSalesItem(id);
        }

        public bool addSalesItem(SalesItem item)
        {
            if (test)
                return _repository.addSalesItem(item);
            else
                return db.addSalesItem(item);
        }   

        public bool removeSalesItem(SalesItem item)
        {
            if (test)
                return _repository.removeSalesItem(item);
            else
                return db.removeSalesItem(item);
        }

        public bool editSalesItem(SalesItem item)
        {
            if (test)
                return _repository.editSalesItem(item);
            else
                return db.editSalesItem(item);
        }

        public List<SalesItem> getSalesItemsWithGenre()
        {
            if (test)
                return _repository.getSalesItemsWithGenre();
            else
                return db.getSalesItemsWithGenre();
        }
    }
}
