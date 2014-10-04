using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using WebAppH2014.Models;

namespace WebApp.Models
{
    public class Order
    {

        public int OrderId { get; set; }
        //TODO get these items from it's own database.  where ordernr=this select *
        public ICollection<OrderSalesItem> SalesItems { get; set; }
        public User ownerUser { get; set; }


        //TODO create reciept toString for showing on screen after purchase
        public String toString()
        {

            return "";
        }


    }
}