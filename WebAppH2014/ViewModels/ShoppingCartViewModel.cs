using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppH2014.Models;

namespace WebAppH2014.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}