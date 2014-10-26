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
    }
}
