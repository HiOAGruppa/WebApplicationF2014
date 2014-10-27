using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;
using System.Web;
using System.Web.Mvc;

namespace BLL
{
    public class CartBLL
    {
        StoreContext db = new StoreContext();

        public void AddToCart(SalesItem item)
        {
            db.AddToCart(item);
        }

        public int RemoveFromCart(int id)
        {
            return db.RemoveFromCart(id);
        }

        public List<Cart> GetCartItems()
        {
            return db.GetCartItems();
        }

        public int GetCartItemCount()
        {
            return db.GetCartItemCount();
        }
        public decimal GetCartItemTotal()
        {
            return db.GetCartItemTotal();
        }
        public void EmptyCart()
        {
            db.EmptyCart();
        }


        string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        public static CartBLL GetCart(HttpContextBase context)
        {
            var cart = new CartBLL();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static CartBLL GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }


    }
}
