using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Model;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;

namespace DAL
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
        }

        public List<SalesItem> searchSalesItems(String nameQuery)
        {
           var items = SalesItems.Where(it => it.Name.Contains(nameQuery)).ToList();
           return items;
        }

        string ShoppingCartId { get; set; }
        public void AddToCart(SalesItem item)
        {
            // Get the matching cart and item instances
            var cartItem = Carts.SingleOrDefault(
                    c => c.CartId == ShoppingCartId
                       && c.SalesItemId == item.SalesItemId);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    SalesItemId = item.SalesItemId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }

            // Save changes
            SaveChanges();
        }


        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.CartItemId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    Carts.Remove(cartItem);
                }

                // Save changes
                SaveChanges();
            }

            return itemCount;
        }
        public List<Cart> GetCartItems()
        {
            return Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCartItemCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();

            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetCartItemTotal()
        {
            decimal? total = (from cartItems in Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.Item.Price).Sum();
            return total ?? decimal.Zero;
        }

        public void EmptyCart()
        {
            var cartItems = Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                Carts.Remove(cartItem);
            }

            // Save changes
            SaveChanges();
        }

        //Store 

        public Genre getSelectedGenre(string genre)
        {
            var genreModel = Genres.Include("Items").Single(g => g.Name == genre);
            return genreModel;
        }

    }

}