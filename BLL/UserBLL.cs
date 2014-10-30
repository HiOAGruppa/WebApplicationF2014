using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;
using System.Diagnostics;


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
            return db.findUserLoginByPassword(passwordhash, username);
        }

        public void addUser(User user, UserLogin login)
        {
            db.addUser(user, login);
        }

        public void editUser(int userId, User user)
        {
            db.editUser(userId, user);
        }
        public List<User> getUsers()
        {
            return db.getUsers();
        }


        public void removeUser(User user)
        {
            db.removeUser(user);
        }

        public bool usernameExists(String username)
        {
           return db.usernameExists(username);
        }

        public bool verifyUser(UserModifyUser inUser)
        {
            Debug.WriteLine("In BLL: " + inUser.toString());
            return db.verifyUser(inUser);
        }
    }
}
