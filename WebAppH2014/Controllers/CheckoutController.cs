using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using System.Diagnostics;

namespace WebAppH2014.Controllers
{
    public class CheckoutController : Controller
    {
        UserBLL userDb = new UserBLL();
        SalesItemBLL itemDb = new SalesItemBLL();
        OrderBLL orderDb = new OrderBLL();

        //
        // GET: /Checkout/AddressAndPayment

        public ActionResult AddressAndPayment()
        {
            
            string error = "";
            if (!isLoggedIn())
            {
               // return RedirectToAction("Index", "Login");
                error += "For å fullføre denne handlingen må du være innlogget! Logg inn eller registrer deg, og prøv igjen.";
            }
            else if (CartBLL.GetCart(this.HttpContext).GetCartItems().Count == 0)
            {
                error +="Handlevognen er tom!";
            }
            else {
                int userId = (int)Session["UserId"];
                User currentUser = userDb.getUser(userId);

                if (currentUser.Address == null || currentUser.ZipCode == null)
                {
                    if (!error.Equals(""))
                        error += "\n";
                    error += "Adresse ikke registrert! Registrer adresse på din side, og prøv igjen.";
                }
            }

            if(!error.Equals(""))
            {
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            return View();
        }
        //
        // GET: /Checkout/Complete

        public ActionResult Complete()
        {
            if (CartBLL.GetCart(this.HttpContext).GetCartItems().Count == 0)
                return RedirectToAction("UserPage", "Login");
            if (isLoggedIn())
            {
                int userId = (int)Session["UserId"];
                User currentUser = userDb.getUser(userId);

                var cart = CartBLL.GetCart(this.HttpContext);
                List<Cart> cartList = cart.GetCartItems();
                

                var order = new Order();
                order.UserId = currentUser.UserId;
                Debug.WriteLine("currentUser ID: " + currentUser.UserId);


                List<OrderSalesItem> allOrderItems = new List<OrderSalesItem>();

                foreach(Cart item in cartList )
                {
                    SalesItem sItem = itemDb.findSalesItem(item.SalesItemId);
                    OrderSalesItem osItem = new OrderSalesItem()
                    {
                        SalesItemId = sItem.SalesItemId,
                        OrderId = order.OrderId,
                        Amount = item.Count
                    };

                    allOrderItems.Add(osItem);
                    sItem.InStock -= item.Count;
                    itemDb.editSalesItem(sItem);
                   // orderDb.addSalesItemInOrder(osItem);
                }
                order.SalesItems = allOrderItems;

        //        currentUser.Orders.Add(order);
                orderDb.addOrder(order);

                order.ownerUser = currentUser;
                order.SalesItems = allOrderItems;


         
                CartBLL.GetCart(this.HttpContext).EmptyCart();
                return View(order);
            }
            
            ViewBag.ErrorMessage = "Du må være logget inn for å se dette...";
            return View("Error");
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