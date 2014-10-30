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

            Debug.WriteLine("Cart.Itemcount =" + count);
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetCartItemTotal()
        {
            decimal? total = (from cartItems in Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.Item.Price).Sum();
            Debug.WriteLine("Cart.TotalCost =" + total);
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

        public void addOrder(Order order)
        {
            Orders.Add(order);
            SaveChanges();
        }

        public List<User> getUsers()
        {
            var users = Users.Include(u => u.UserLogin).ToList();//.Include(a => a.Orders)
            return users;
        }

        public Order getOrder(int orderId)
        {
            return Orders.Where(a => a.OrderId == orderId).FirstOrDefault();
        }

        public void addSalesItemInOrder(OrderSalesItem item)
        {
            SalesItemInOrder.Add(item);
            SaveChanges();
        }

        public Order getOrderWithItems(int orderId)
        {
            Order order = Orders.Include("SalesItems").ToList().Single(a => a.OrderId == orderId);
            Debug.WriteLine(order.SalesItems.ToString());
            return order;
        }

        public void removeOrder(int id)
        {
            Order order = getOrder(id);
            Orders.Remove(order);
            SaveChanges();
        }

        public List<Order> getOrders()
        {
            var orders = Orders.Include(s => s.SalesItems.Select(i => i.SalesItem)).Include("ownerUser").ToList();
            return orders;
        }

        public void addSalesItem(SalesItem item)
        {
            SalesItems.Add(item);
            SaveChanges();
        }

        public void removeSalesItem(SalesItem item)
        {
            SalesItems.Remove(item);
            SaveChanges();
        }

        public void editSalesItem(SalesItem item)
        {
            Entry(item).State = EntityState.Modified;
            SaveChanges();
        }

        public List<SalesItem> getSalesItemsWithGenre()
        {
            return SalesItems.Include(a => a.Genre).ToList();
        }

        public UserLogin findUserLoginByPassword(byte[] passwordhash, String username)
        {
            return UserPasswords.Where(b => b.Password == passwordhash && b.UserName == username).FirstOrDefault();
        }

        public void addUser(User user, UserLogin login)
        {
            Users.Add(user);
            UserPasswords.Add(login);
            SaveChanges();
        }


        public void editUser(int userId, User user)
        {
            User oldUser = getUser(userId);
            oldUser = user;
            SaveChanges();
        }

        public void removeUser(User user)
        {
            Users.Remove(user);
            SaveChanges();
        }

        public bool isUserInDB(UserModifyUser inUser)
        {
            Debug.WriteLine("In Context: " + inUser.toString());

            if (inUser.OldPassword == null || inUser.UserLogin.UserName == null)
                return false;
            //these fields are dependent on index.cshtml modelformat used to generate the inUser
            byte[] passordDb = genHash(inUser.OldPassword);
            UserLogin foundUser = findUserLoginByPassword(passordDb, inUser.UserLogin.UserName);
            if (foundUser == null)
            {
                return false;
            }
            else
            {
                inUser.UserId = foundUser.UserId;
                return true;
            }

        }

        private static byte[] genHash(string inPassword)
        {
            byte[] inData, outData;
            var algorithm = System.Security.Cryptography.SHA256.Create();
            inData = System.Text.Encoding.ASCII.GetBytes(inPassword);
            outData = algorithm.ComputeHash(inData);
            return outData;
        }

    }

}