using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppH2014.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebAppH2014.Models
{
    public class SalesItem
    {
        //TODO Complete class in accordance with what DB+View needs.
        public int SalesItemId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int InStock { get; set; }
        public int? GenreId { get; set; }
        public Genre Genre { get; set; }
        public ICollection<OrderSalesItem> IncludedInOrders { get; set; }



    }
}