using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppH2014.Models;

namespace WebApp.Models
{
    public class SalesItem
    {
        //TODO Complete class in accordance with what DB+View needs.

        public int SalesItemId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int InStock { get; set; }

        public ICollection<OrderSalesItem> IncludedInOrders { get; set; }



    }
}