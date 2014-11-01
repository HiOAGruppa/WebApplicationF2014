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

        //gets all orders associated with the user in question
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
            Debug.WriteLine("Database-change: Added order, of type List<SalesItem>, to SalesItemInOrder");
        }

        //Method for searching for a given item with % Text % in db
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
            Debug.WriteLine("Database-change: Added SalesItem(" + item.Name + ") to cart");
        }

        //removes a single item from the shoppingcart that's bound to session.
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
                Debug.WriteLine("Database-change: Removed Item(" + cartItem.Item.Name + ") from cart");
            }

            return itemCount;
        }
        //gets all items in the shoppingcart
        public List<Cart> GetCartItems()
        {
            return Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        //finds number of items in the shoppingcart
        public int GetCartItemCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();

            Debug.WriteLine("Cart.Itemcount = " + count);
            // Return 0 if all entries are null
            return count ?? 0;
        }
        //gets the total sum for the items in shoppingcart
        public decimal GetCartItemTotal()
        {
            decimal? total = (from cartItems in Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.Item.Price).Sum();
            Debug.WriteLine("Cart.TotalCost = " + total);
            return total ?? decimal.Zero;
        }

        //empties out the shoppingcart associated with session. Always called after purchase.
        public void EmptyCart()
        {
            var cartItems = Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                Carts.Remove(cartItem);
            }

            // Save changes
            SaveChanges();
            Debug.WriteLine("Database-change: Cart emptied");
        }

        public bool addSalesItem(SalesItem item)
        {
            SalesItems.Add(item);
            SaveChanges();
            Debug.WriteLine("Database-change: Added SalesItem (" + item.Name + ") to database");
            return true;
        }

        public bool removeSalesItem(SalesItem item)
        {
            SalesItems.Remove(item);
            SaveChanges();
            Debug.WriteLine("Database-change: Removed SalesItem (" + item.Name + ") from database");
            return true;
        }

        public bool editSalesItem(SalesItem item)
        {
            Entry(item).State = EntityState.Modified;
            SaveChanges();
            Debug.WriteLine("Database-change: Edited SalesItem (" + item.Name + ") in database");
            return true;
        }

        public List<SalesItem> getSalesItemsWithGenre()
        {
            return SalesItems.Include(a => a.Genre).ToList();
        }

        public SalesItem findSalesItem(int id)
        {
            return SalesItems.Find(id);
        }



        //Store 


        public Genre getSelectedGenre(string genre)
        {
            var genreModel = Genres.Include("Items").Single(g => g.Name == genre);
            return genreModel;
        }

        //adds an order to the database
        public void addOrder(Order order)
        {
            Orders.Add(order);
            SaveChanges();
            Debug.WriteLine("Database-change: Added order, of type Order");
        }

        //get all users
        public List<User> getUsers()
        {
            var users = Users.Include(u => u.UserLogin).ToList();//.Include(a => a.Orders)
            return users;
        }

        public Order getOrder(int orderId)
        {
            return Orders.Where(a => a.OrderId == orderId).FirstOrDefault();
        }

        //add a single salesitem in an order.
        public void addSalesItemInOrder(OrderSalesItem item)
        {
            SalesItemInOrder.Add(item);
            SaveChanges();
            Debug.WriteLine("Database-change: Added OrderSalesItem (" + item.SalesItem.Name + ") to SalesItemInOrder");
        }
        //get an order and include the salesitems in it.
        public Order getOrderWithItems(int orderId)
        {
            Order order = Orders.Include("SalesItems").ToList().Single(a => a.OrderId == orderId);
            Debug.WriteLine(order.SalesItems.ToString());
            return order;
        }
        //remove an order.
        public void removeOrder(int id)
        {
            Order order = getOrder(id);
            Orders.Remove(order);
            SaveChanges();
            Debug.WriteLine("Database-change: Removed order beloning to (" + order.ownerUser.UserLogin.UserName + ")");
        }

        public List<Order> getOrders()
        {
            var orders = Orders.Include(s => s.SalesItems.Select(i => i.SalesItem)).Include("ownerUser").ToList();
            return orders;
        }

        //finds if a user with a given username and password in the database
        public UserLogin findUserLoginByPassword(byte[] passwordhash, String username)
        {
            return UserPasswords.Where(b => b.Password == passwordhash && b.UserName == username).FirstOrDefault();
        }

        public void addUser(User user, UserLogin login)
        {
            Users.Add(user);
            UserPasswords.Add(login);
            SaveChanges();
            Debug.WriteLine("Database-change: Added User (" + login.UserName + ") to database");
        }


        public void editUser(int userId, User user)
        {
            User oldUser = getUser(userId);
            oldUser = user;
            SaveChanges();
            Debug.WriteLine("Database-change: Edited User (" + user.UserLogin.UserName + ") in database");
        }

        public void removeUser(User user)
        {
            Users.Remove(user);
            SaveChanges();
            Debug.WriteLine("Database-change: Removed User (" + user.UserLogin.UserName + ") from database");
        }

        //Does the username already exist in the db? Used for checking on register of new user.
        public bool usernameExists(String username)
        {
            UserLogin user = UserPasswords.Where(a => a.UserName.Equals(username)).SingleOrDefault();

            if (user != null)
                return true;
            else
                return false;
        }

        //verifies a user when logging in. Is user in db? user not null
        public bool verifyUser(UserModifyUser inUser)
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

        //method used to generate a hashed password to match it up against the database.
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