using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace WebApp.Models
{
    public class Order
    {
        //TODO get these items from it's own database.  where ordernr=this select *
        public ArrayList SalesItems { get; set; }
        public int BelongsToCustomerID { get; set; }

        public Order(int CustomerID)
        {
            BelongsToCustomerID = CustomerID;
            SalesItems = new ArrayList();
        }

        public void addSalesItemToOrder(SalesItem item)
        {
            SalesItems.Add(item);
            return;
        }

        //TODO create reciept toString for showing on screen after purchase
        public String toString()
        {

            return "";
        }


    }
}