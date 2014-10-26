using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    class OrderBLL
    {

        public List<Model.Order> getUserOrders(int userId)
        {
            StoreContext db = new StoreContext();
            return db.getUserOrders(userId);
        }

    }
}
