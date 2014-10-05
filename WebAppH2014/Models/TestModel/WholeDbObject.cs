using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebAppH2014.Models.TestModel
{
    public class WholeDbObject
    {
        public User User { get; set; }
        public List<Order> Orders { get; set; }

    }
}