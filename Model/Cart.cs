using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Cart
    {
        [Key]
        public int CartItemId { get; set; }
        public string CartId { get; set; }
        public int SalesItemId { get; set; }
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual SalesItem Item { get; set; }
    }
}