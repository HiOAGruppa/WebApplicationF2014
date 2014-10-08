using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppH2014.Models;

namespace WebAppH2014.Controllers
{
    public class CheckoutController : Controller
    {
        StoreContext storeDB = new StoreContext();

        //
        // GET: /Checkout/AddressAndPayment

        public ActionResult AddressAndPayment()
        {
            if (ShoppingCart.GetCart(this.HttpContext).GetCartItems().Count == 0)
            {
                string error ="Din handlevogn er tom!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            if (!isLoggedIn())
                return RedirectToAction("Index","Login");
            else
                return View();
        }
        //
        // GET: /Checkout/Complete

        public ActionResult Complete()
        {
            if (isLoggedIn())
            {
                int userId = (int)Session["UserId"];
                User currentUser = storeDB.getUser(userId);

                var cart = ShoppingCart.GetCart(this.HttpContext);
                List<Cart> cartList = cart.GetCartItems();
                

                var order = new Order();
                order.ownerUser = currentUser;

                foreach(Cart item in cartList )
                {
                    SalesItem sItem = storeDB.SalesItems.Find(item.SalesItemId);
                    OrderSalesItem osItem = new OrderSalesItem()
                    {
                        SalesItemId = sItem.SalesItemId,
                        OrderId = order.OrderId,
                        Amount = item.Count,
                        SalesItem = sItem,
                        Order = order
                    };

                    storeDB.SalesItemInOrder.Add(osItem);

                }
                currentUser.Orders.Add(order);

                storeDB.Orders.Add(order);
                storeDB.SaveChanges();
            }
            ShoppingCart.GetCart(this.HttpContext).EmptyCart();
            return View();
        }

        private Boolean isLoggedIn()
        {
            if (Session["LoggedIn"] != null)
            {
                if (ViewBag.isUser != 0)
                    return (bool)Session["LoggedIn"];
                else
                    return false;
            }
            return false;
        }
    }
}