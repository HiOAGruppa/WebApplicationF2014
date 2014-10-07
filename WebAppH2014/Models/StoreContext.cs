using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebAppH2014.Models;

namespace WebAppH2014.Models
{
    public class StoreContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderSalesItem> SalesItemInOrder { get; set; }

        public DbSet<SalesItem> SalesItems { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<UserLogin> UserPasswords { get; set; }

        public DbSet<Cart> Carts { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        //gets a user with complete sub-information
        //could technically return null
        public User getUser(int userId)
        {
            //get user
            User user = Users.Where(it => it.UserId == userId).First();
            getUserOrders(userId);
            UserLogin userlogin = UserPasswords.Where(it => it.UserId == userId).First();

            return user;
        }

        public List<Order> getUserOrders(int userId)
        {
            //get all orders of the user
            List<Order> orders = Orders.Where(it => it.ownerUser.UserId == userId).ToList();
            //get all SalesItems
            var allSalesItems = getAllSalesItems();

            //add all the salesitems in each order, also couple them with the real SalesItem
            foreach (var order in orders)
            {
                addOrderSalesItems(order, allSalesItems);
            }

            return orders;
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
            //foreach (var item in salesItemsInOrder)
            //{
            //    var salesitem = allItems.Where(it => it.SalesItemId == item.SalesItemId).First();
            //    item.SalesItem = salesitem;
            //}
            //order.SalesItems = salesItemsInOrder;
        }



    }

}