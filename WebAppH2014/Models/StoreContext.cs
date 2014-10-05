using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebAppH2014.Models
{
    public class StoreContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderSalesItem> SalesItemInOrder { get; set; }

        public DbSet<SalesItem> SalesItems { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        //gets a user with complete sub-information
        public User getUser(int userId)
        {
            User user = Users.Where(it => it.UserId == userId).First();

            List<Order> orders = Orders.Where(it => it.ownerUser.UserId == userId).ToList();
            var allSalesItems = getAllSalesItems();

            foreach (var order in orders)
            {
                addOrderSalesItems(order, allSalesItems);
            }

            return user;
        }

        public List<SalesItem> getAllSalesItems()
        {
            List<SalesItem> salesItems = SalesItems.ToList();
            return salesItems;
        }

        //gets all the salesitems in a given order and adds them
        private void addOrderSalesItems(Order order, List<SalesItem> allItems)
        {
            var salesItemsInOrder = SalesItemInOrder.Where(it => it.OrderId == order.OrderId).ToList();
            foreach (var item in salesItemsInOrder)
            {
                var salesitem = allItems.Where(it => it.SalesItemId == item.SalesItemId).First();
                item.SalesItem = salesitem;
            }
            order.SalesItems = salesItemsInOrder;
        }



    }

}