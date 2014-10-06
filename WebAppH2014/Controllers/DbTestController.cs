using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using WebAppH2014.Models;
using WebAppH2014.Models.TestModel;

namespace WebAppH2014.Controllers
{
    public class DbTestController : Controller
    {

        private StoreContext db = new StoreContext();

        // GET: DbTest
        public ActionResult Index()
        {

            //Complicated way to get all database-items needed to display our order-history
            //gets all users and salesitems
            var users = db.Users.ToList();
            var salesItems = db.SalesItems.Include(a => a.Genre).ToList();
            //foreach (var user in users)
            //{

            //    //gets all orders belonging to current user
            //    var orders = db.Orders.Where(it => it.ownerUser.UserId == user.UserId).ToList();
            //    Debug.WriteLine("Orders found");

            //    foreach (var order in orders)
            //    {
            //        //adds the list of items in the order
            //        var orderSalesItem = db.SalesItemInOrder.Where(it => it.OrderId == order.OrderId).ToList();
            //        foreach(var item in orderSalesItem)
            //        {
            //            //couples the items in the order, to a salesItem-object which will give us the item-info
            //            var salesitem = salesItems.Where(it => it.SalesItemId == item.SalesItemId).FirstOrDefault();
            //            item.SalesItem = salesitem;
            //        }
            //        //adds all the orderItems to the order
            //        order.SalesItems = orderSalesItem;
            //    }
            //    //adds the orders to the user
            //    user.Orders = orders;
            //}
            List<User> theseusers = new List<User>();
            foreach (var user in users)
            {
                theseusers.Add(db.getUser(user.UserId));
            }


            return View(theseusers);
        }


        public ActionResult Stock()
        {
            var users = db.Users.ToList();
            var salesItems = db.SalesItems.ToList();
            foreach (var user in users)
            {

                //gets all orders belonging to current user
                var orders = db.Orders.Where(it => it.ownerUser.UserId == user.UserId).ToList();
                Debug.WriteLine("Orders found");

                foreach (var order in orders)
                {
                    //adds the list of items in the order
                    var orderSalesItem = db.SalesItemInOrder.Where(it => it.OrderId == order.OrderId).ToList();
                    foreach (var item in orderSalesItem)
                    {
                        //couples the items in the order, to a salesItem-object which will give us the item-info
                        var salesitem = salesItems.Where(it => it.SalesItemId == item.SalesItemId).FirstOrDefault();
                        item.SalesItem = salesitem;
                    }
                    //adds all the orderItems to the order
                    order.SalesItems = orderSalesItem;
                }
                //adds the orders to the user
                user.Orders = orders;
            }

            return View(salesItems);
        }
    }
}