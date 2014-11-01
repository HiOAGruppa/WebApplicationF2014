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
    public class StoreManagerRepository : DAL.IStoreManagerRepository
    {
        StoreContext db = new StoreContext();
        string filename = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "logErrors.txt";

        public bool addSalesItem(SalesItem item)
        {
            try {
                db.SalesItems.Add(item);
                db.SaveChanges();
                Debug.WriteLine("Database-change: Added SalesItem (" + item.Name + ") to database");
                return true;
            }
            catch (Exception e)
            {
                var sw = new System.IO.StreamWriter(filename, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
                return false;
            }
        }

        public bool removeSalesItem(SalesItem item)
        {
            try
            {
                string name = item.Name;
                db.SalesItems.Remove(item);
                db.SaveChanges();
                Debug.WriteLine("Database-change: Removed SalesItem (" + name + ") from database");
                return true;
            }
            catch (Exception e)
            {
                var sw = new System.IO.StreamWriter(filename, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
                return false;
            }
        }

        public bool editSalesItem(SalesItem item)
        {
            try
            {
                item = null;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                Debug.WriteLine("Database-change: Edited SalesItem (" + item.Name + ") in database");
                return true;
            }
            catch (Exception e)
            {
                var sw = new System.IO.StreamWriter(filename, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
                return false;
            }
        }

        public List<SalesItem> getSalesItemsWithGenre()
        {
            try {
                return db.SalesItems.Include(a => a.Genre).ToList();
            }
            catch (Exception e)
            {
                var sw = new System.IO.StreamWriter(filename, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
                return null;
            }
        }


        //get all users
        public List<User> getUsers()
        {
            try {
                var users = db.Users.Include(u => u.UserLogin).ToList();//.Include(a => a.Orders)
                return users;
            }
            catch (Exception e)
            {
                var sw = new System.IO.StreamWriter(filename, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
                return null;
            }
        }

        public List<Order> getOrders()
        {
            try {
                var orders = db.Orders.Include(s => s.SalesItems.Select(i => i.SalesItem)).Include("ownerUser").ToList();
                return orders;
            }
            catch (Exception e)
            {
                var sw = new System.IO.StreamWriter(filename, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
                return null;
            }
        }

        public void editUser(int userId, User user)
        {
            try {
                User oldUser = db.getUser(userId);
                oldUser = user;
                db.SaveChanges();
                Debug.WriteLine("Database-change: Edited User (" + user.UserLogin.UserName + ") in database");
            }
            catch (Exception e)
            {
                var sw = new System.IO.StreamWriter(filename, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
            }
        }

        public void removeUser(User user)
        {
            try {
                string name = user.UserLogin.UserName;
                db.Users.Remove(user);
                db.SaveChanges();
                Debug.WriteLine("Database-change: Removed User (" + name + ") from database");
            }
            catch (Exception e)
            {
                var sw = new System.IO.StreamWriter(filename, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
            }
        }
    }
}
