﻿using System;
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
                    SalesItem newItem = item.Item;
                    OrderSalesItem osItem = new OrderSalesItem()
                    {
                        SalesItemId = newItem.SalesItemId,
                        OrderId = order.OrderId,
                        Amount = item.Count,
                        SalesItem = newItem,
                        Order = order
                    };

                    storeDB.SalesItemInOrder.Add(osItem);

                }
                currentUser.Orders.Add(order);

                storeDB.Orders.Add(order);
                storeDB.SaveChanges();
            }
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