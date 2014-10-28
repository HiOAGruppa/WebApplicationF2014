using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;

namespace BLL
{
    public class UserBLL
    {
        StoreContext db = new StoreContext();
        public User getUser(int userId)
        {
            return db.getUser(userId);
        }

        public UserLogin findUserLoginByPassword(byte[] passwordhash, String username)
        {
            return db.UserPasswords.Where(b => b.Password == passwordhash && b.UserName == username).FirstOrDefault();
        }

        public void addUser(User user, UserLogin login)
        {
            db.Users.Add(user);
            db.UserPasswords.Add(login);
            db.SaveChanges();
        }

        public void editUser(int userId, User user)
        {
            User oldUser = getUser(userId);
            oldUser = user;
            db.SaveChanges();

        }
        public List<User> getUsers()
        {
            return db.getUsers();
        }


        public void removeUser(User user)
        {
            db.Users.Remove(user);
            db.SaveChanges();
        }
    }
}
