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

        public bool addSalesItem(SalesItem item)
        {
            db.SalesItems.Add(item);
            db.SaveChanges();
            Debug.WriteLine("Database-change: Added SalesItem (" + item.Name + ") to database");
            return true;
        }

        public bool removeSalesItem(SalesItem item)
        {
            string name = item.Name;
            db.SalesItems.Remove(item);
            db.SaveChanges();
            Debug.WriteLine("Database-change: Removed SalesItem (" + name + ") from database");
            return true;
        }

        public bool editSalesItem(SalesItem item)
        {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            Debug.WriteLine("Database-change: Edited SalesItem (" + item.Name + ") in database");
            return true;
        }

        public List<SalesItem> getSalesItemsWithGenre()
        {
            return db.SalesItems.Include(a => a.Genre).ToList();
        }


        //get all users
        public List<User> getUsers()
        {
            var users = db.Users.Include(u => u.UserLogin).ToList();//.Include(a => a.Orders)
            return users;
        }


        public List<Order> getOrders()
        {
            var orders = db.Orders.Include(s => s.SalesItems.Select(i => i.SalesItem)).Include("ownerUser").ToList();
            return orders;
        }

        public void editUser(int userId, User user)
        {
            User oldUser = db.getUser(userId);
            oldUser = user;
            db.SaveChanges();
            Debug.WriteLine("Database-change: Edited User (" + user.UserLogin.UserName + ") in database");
        }

        public void removeUser(User user)
        {
            string name = user.UserLogin.UserName;
            db.Users.Remove(user);
            db.SaveChanges();
            Debug.WriteLine("Database-change: Removed User (" + name + ") from database");
        }
    }
}
