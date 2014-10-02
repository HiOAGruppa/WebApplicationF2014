using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppH2014.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Address { get; set; }
        public int ZipCode { get; set; }

        public DateTime DateOfBirth { get; set; }

        //TODO Passord, Auth, DB generation

    }
}