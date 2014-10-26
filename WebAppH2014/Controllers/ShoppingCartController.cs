using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppH2014.ViewModels;
using BLL;


namespace WebAppH2014.Controllers
{

    public class ShoppingCartController : Controller
    {
        CartBLL shoppingcartDb = new CartBLL();
        SalesItemBLL itemdb = new SalesItemBLL();

        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            var cart = CartBLL.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = shoppingcartDb.GetCartItems(),
                CartTotal = shoppingcartDb.GetCartItemTotal()
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5

        public ActionResult AddToCart(int id)
        {
            var addedItem = itemdb.findSalesItem(id);

            // Add it to the shopping cart
            var cart = CartBLL.GetCart(this.HttpContext);

            shoppingcartDb.AddToCart(addedItem);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = CartBLL.GetCart(this.HttpContext);
            int itemCount = shoppingcartDb.RemoveFromCart(id);


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
            var cart = CartBLL.GetCart(this.HttpContext);

            ViewData["CartCount"] = shoppingcartDb.GetCartItemCount();

            return PartialView("CartSummary");
        }
    }
}