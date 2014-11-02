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

        string fileError = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "logErrors.txt";
        string fileChange = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "logChanges.txt";

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            }
            catch (Exception e)
            {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
            }
        }

        //gets a user with complete sub-information
        //could technically return null
        public User getUser(int userId)
        {
            //get user
            try {
                User user = Users.Where(it => it.UserId == userId).First();
                getUserOrders(userId);
                UserLogin userlogin = UserPasswords.Where(it => it.UserId == userId).First();
                return user;
            }
            catch (Exception e)
            {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }

        //gets all orders associated with the user in question
        public List<Order> getUserOrders(int userId)
        {
            try {
                //get all orders of the user
                List<Order> orders = Orders.Where(it => it.ownerUser.UserId == userId).ToList();
                //get all SalesItems
                var allSalesItems = getAllSalesItems();
                //add all the salesitems in each order, also couple them with the real SalesItem
                foreach (var order in orders) {
                    addOrderSalesItems(order, allSalesItems);
                }
                return orders;
            }
            catch (Exception e)
            {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }

        public List<SalesItem> getAllSalesItems()
        {
            try {
                List<SalesItem> salesItems = SalesItems.ToList();
                return salesItems;
            }
            catch (Exception e)
            {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }

        //gets all the salesitems in a given order and adds them
        private void addOrderSalesItems(Order order, List<SalesItem> allItems)
        {
            try {
                var salesItemsInOrder = SalesItemInOrder.Where(it => it.OrderId == order.OrderId).ToList();
                logChange("Database-change: Added order, of type List<SalesItem>, with length " + allItems.Count + ", to SalesItemInOrder");
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
            }       
        }

        //Method for searching for a given item with % Text % in db
        public List<SalesItem> searchSalesItems(String nameQuery)
        {
            try {
               var items = SalesItems.Where(it => it.Name.Contains(nameQuery)).ToList();
               return items;
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }

        string ShoppingCartId { get; set; }
        public void AddToCart(SalesItem item)
        {
            try {
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
                logChange("Database-change: Added SalesItem(" + item.Name + ") to cart");
            }
            catch (Exception e)
            {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
            }
        }

        //removes a single item from the shoppingcart that's bound to session.
        public int RemoveFromCart(int id)
        {
            try {
                // Get the cart
                var cartItem = Carts.Single(
                    cart => cart.CartId == ShoppingCartId
                    && cart.CartItemId == id);

                string name = "";
                int itemCount = 0;

                if (cartItem != null)
                {
                    name = cartItem.Item.Name;
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
                    if (!name.Equals("")) logChange("Database-change: Removed Item(" + name + ") from cart");
                }
                return itemCount;
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return 0;
            }
        }
        //gets all items in the shoppingcart
        public List<Cart> GetCartItems()
        {
            try {
                return Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }

        //finds number of items in the shoppingcart
        public int GetCartItemCount()
        {
            try
            {
                // Get the count of each item in the cart and sum them up
                int? count = (from cartItems in Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count).Sum();

                Debug.WriteLine("Cart.Itemcount = " + count);
                // Return 0 if all entries are null
                return count ?? 0;
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return 0;
            }
        }
        //gets the total sum for the items in shoppingcart
        public decimal GetCartItemTotal()
        {
            try {
                decimal? total = (from cartItems in Carts
                                  where cartItems.CartId == ShoppingCartId
                                  select (int?)cartItems.Count * cartItems.Item.Price).Sum();
                Debug.WriteLine("Cart.TotalCost = " + total);
                return total ?? decimal.Zero;
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return 0;
            }
        }

        //empties out the shoppingcart associated with session. Always called after purchase.
        public void EmptyCart()
        {
            try {
                var cartItems = Carts.Where(cart => cart.CartId == ShoppingCartId);

                foreach (var cartItem in cartItems)
                {
                    Carts.Remove(cartItem);
                }

                // Save changes
                SaveChanges();
                logChange("Database-change: Cart emptied");
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
            }
        }

        public bool addSalesItem(SalesItem item)
        {
            try {
                SalesItems.Add(item);
                SaveChanges();
                logChange("Database-change: Added SalesItem (" + item.Name + ") to database");
                return true;
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return false;
            }
        }

        public bool removeSalesItem(SalesItem item)
        {
            try {
                SalesItems.Remove(item);
                SaveChanges();
                logChange("Database-change: Removed SalesItem (" + item.Name + ") from database");
                return true;
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return false;
            }
        }

        public bool editSalesItem(SalesItem item)
        {
            try {
                Entry(item).State = EntityState.Modified;
                SaveChanges();
                logChange("Database-change: Edited SalesItem (" + item.Name + ") in database");
                return true;
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return false;
            }
        }

        public List<SalesItem> getSalesItemsWithGenre()
        {
            try {
                return SalesItems.Include(a => a.Genre).ToList();
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }

        public SalesItem findSalesItem(int id)
        {
            try {
                return SalesItems.Find(id);
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }


        //Store 
        public Genre getSelectedGenre(string genre)
        {
            try {
                var genreModel = Genres.Include("Items").Single(g => g.Name == genre);
                return genreModel;
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }

        //adds an order to the database
        public void addOrder(Order order)
        {
            try {
                Orders.Add(order);
                SaveChanges();
                logChange("Database-change: Added order, with length " + order.SalesItems.Count + ",of type Order");
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
            }
        }

        //get all users
        public List<User> getUsers()
        {
            try {
                var users = Users.Include(u => u.UserLogin).ToList();//.Include(a => a.Orders)
                return users;
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }

        public Order getOrder(int orderId)
        {
            try {
                return Orders.Where(a => a.OrderId == orderId).FirstOrDefault();
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }

        //add a single salesitem in an order.
        public void addSalesItemInOrder(OrderSalesItem item)
        {
            try {
                SalesItemInOrder.Add(item);
                SaveChanges();
                logChange("Database-change: Added OrderSalesItem (" + item.SalesItem.Name + ") to SalesItemInOrder");
            }
            catch (Exception e) {
                logError((DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException));
            }
        }
        //get an order and include the salesitems in it.
        public Order getOrderWithItems(int orderId)
        {
            try {
                Order order = Orders.Include("SalesItems").ToList().Single(a => a.OrderId == orderId);
                Debug.WriteLine(order.SalesItems.ToString());
                return order;
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }
        //remove an order.
        public bool removeOrder(int id)
        {
            var ok = true;
            try {
                Order order = getOrder(id);
                Orders.Remove(order);
                SaveChanges();
                logChange("Database-change: Removed order (" + id + ")");
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                ok = false;
            }
            return ok;
        }

        public List<Order> getOrders()
        {
            try {
                var orders = Orders.Include(s => s.SalesItems.Select(i => i.SalesItem)).Include("ownerUser").ToList();
                return orders;
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }

        //finds if a user with a given username and password in the database
        public UserLogin findUserLoginByPassword(byte[] passwordhash, String username)
        {
            try {
                return UserPasswords.Where(b => b.Password == passwordhash && b.UserName == username).FirstOrDefault();
            }
            catch (Exception e) {
                logError((DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException));
                return null;
            }
        }

        public void addUser(User user, UserLogin login)
        {
            try {
                Users.Add(user);
                UserPasswords.Add(login);
                SaveChanges();
                logChange("Database-change: Added User (" + login.UserName + ") to database");
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
            }
        }

        public bool editUser(int userId, User user)
        {
            var ok = true;
            try {
                User oldUser = getUser(userId);
                oldUser = user;
                SaveChanges();
                logChange("Database-change: Edited User (" + user.UserLogin.UserName + ") in database");
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                ok = false;
            }
            return ok;
        }


        public bool removeUser(User user)
        {
            var ok = true;
            try {
                string name = user.UserLogin.UserName;
                UserPasswords.Remove(user.UserLogin);
                Users.Remove(user);
                SaveChanges();
                logChange("Database-change: Removed User (" + name + ") from database");
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                ok = false;
            }
            return ok;
        }

        //Does the username already exist in the db? Used for checking on register of new user.
        public bool usernameExists(String username)
        {
            try {
                UserLogin user = UserPasswords.Where(a => a.UserName.Equals(username)).SingleOrDefault();

                if (user != null)
                    return true;
                else
                    return false;
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return false;
            }
        }

        //verifies a user when logging in. Is user in db? user not null
        public bool verifyUser(UserModifyUser inUser) {
            try {
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
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return false;
            }
        }

        //method used to generate a hashed password to match it up against the database.
        private static byte[] genHash(string inPassword) {
            byte[] inData, outData;
            var algorithm = System.Security.Cryptography.SHA256.Create();
            inData = System.Text.Encoding.ASCII.GetBytes(inPassword);
            outData = algorithm.ComputeHash(inData);
            return outData;
        }

        private void logError(String message)
        {
            var sw = new System.IO.StreamWriter(fileError, true);
            sw.WriteLine(message);
            sw.Close();
        }

        private void logChange(String message)
        {
            var sw = new System.IO.StreamWriter(fileChange, true);
            sw.WriteLine(DateTime.Now.ToString() + Environment.NewLine + message);
            sw.Close();
        }
    }
}