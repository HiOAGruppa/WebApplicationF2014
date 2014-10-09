using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppH2014.Models;
using WebAppH2014.ViewModels;

namespace WebAppH2014.Controllers
{

    public class ShoppingCartController : Controller
    {
        StoreContext storeDB = new StoreContext();

        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5

        public ActionResult AddToCart(int id)
        {
            var addedItem = storeDB.SalesItems
                .Single(i => i.SalesItemId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedItem);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int itemCount = cart.RemoveFromCart(id);


            return RedirectToAction("Index");
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
        //
        // GET: /ShoppingCart/CartSummary

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();

            return PartialView("CartSummary");
        }

        [ChildActionOnly]
        public ActionResult LoggedIn()
        {
            string ut = "Logg inn";
            if (isLoggedIn())
                ut = "Min Side";

            ViewData["CartCount"] = ut;

            return PartialView("LoggedSummary");
        }
        [ChildActionOnly]
        public ActionResult LoggedInSidenav()
        {
            string ut = "Logg inn";
            if (isLoggedIn())
                ut = "Min Side";

            ViewData["CartCount"] = ut;

            return PartialView("LoggedSummarySide");
        }
    }
}