using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using System.Diagnostics;
using WebAppH2014.ViewModels;

namespace WebAppH2014.Controllers
{
    public class CheckoutController : Controller
    {
        public ActionResult AddressAndPayment()
        {
            UserBLL userDb = new UserBLL();
            SalesItemBLL itemDb = new SalesItemBLL();
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

                var viewModel = new CheckoutViewModel()
                {
                    PersonId = userId,
                    Firstname = currentUser.FirstName,
                    Lastname = currentUser.LastName,
                    Address = currentUser.Address,
                    Zipcode = currentUser.ZipCode
                };
                return View(viewModel);
            }

            ViewBag.ErrorMessage = error;
            return View("Error");
            
        }
        [HttpPost]
        
        public ActionResult AddressAndPayment(CheckoutViewModel user)
        {
           /* User currentUser = userDb.getUser(viewModel.PersonId);

            currentUser.FirstName = viewModel.Firstname;
            currentUser.LastName = viewModel.Lastname;
            currentUser.Address = viewModel.Address;
            currentUser.ZipCode = viewModel.Zipcode;

            userDb.editUser(currentUser.UserId,currentUser);
            return RedirectToAction("Complete");*/
            UserBLL userDb = new UserBLL();
            int userId = (int)Session["UserId"];
            if (userId != 0)
            {
                User userInDb = userDb.getUser(userId);

                
                //changes all user-settings that differ from db-object
                if (user.Firstname != userInDb.FirstName && user.Firstname != "")
                    userInDb.FirstName = user.Firstname;
                if (user.Lastname != userInDb.LastName && user.Lastname != "")
                    userInDb.LastName = user.Lastname;
                if (user.Zipcode != userInDb.ZipCode)
                    userInDb.ZipCode = user.Zipcode;
                if (user.Address != userInDb.Address)
                    userInDb.Address = user.Address;

                userDb.editUser(userInDb.UserId, userInDb);
            }

            return RedirectToAction("Complete");
        }
        //
        // GET: /Checkout/Complete

        public ActionResult Complete()
        {
            SalesItemBLL itemDb = new SalesItemBLL();
            OrderBLL orderDb = new OrderBLL();
            UserBLL userDb = new UserBLL();
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

                }
                order.SalesItems = allOrderItems;

                orderDb.addOrder(order);
     
                CartBLL.GetCart(this.HttpContext).EmptyCart();
                return View(userDb.getUser(currentUser.UserId).Orders.Last());
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