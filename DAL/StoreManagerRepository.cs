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
    public class StoreManagerRepository : DbContext, DAL.IStoreManagerRepository
    {
        StoreContext db = new StoreContext();
        string fileError = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "logErrors.txt";
        string fileChange = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "logChanges.txt";

        public bool addSalesItem(SalesItem item)
        {
            try {
                db.SalesItems.Add(item);
                db.SaveChanges();
                logChange("Database-change: Added SalesItem (" + item.Name + ") to database");
                return true;
            }
            catch (Exception e)
            {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
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
                logChange("Database-change: Removed SalesItem (" + name + ") from database");
                return true;
            }
            catch (Exception e)
            {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return false;
            }
        }

        public bool editSalesItem(SalesItem item)
        {
            try {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
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
                return db.SalesItems.Include(a => a.Genre).ToList();
            }
            catch (Exception e)
            {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }

        public SalesItem findSalesItem(int id)
        {
            try {
                return db.SalesItems.Find(id);
            }
            catch (Exception e) {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
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
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
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
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                return null;
            }
        }

        public bool editUser(int userId, User user)
        {
            var value = true;
            try {
                User oldUser = db.getUser(userId);
                oldUser = user;
                db.SaveChanges();
                logChange("Database-change: Edited User (" + user.UserLogin.UserName + ") in database");
            }
            catch (Exception e)
            {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                value = false;
            }
            return value;
        }

        public bool removeUser(User user)
        {
            var ok = true;
            try {
                string name = user.UserLogin.UserName;
                db.Users.Remove(user);
                db.SaveChanges();
                logChange("Database-change: Removed User (" + name + ") from database");
            }
            catch (Exception e)
            {
                logError(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                ok = false;
            }
            return ok;
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
