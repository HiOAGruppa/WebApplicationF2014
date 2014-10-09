﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebAppH2014.Models;

namespace WebAppH2014.Models
{
    public class User
    {

         public int UserId { get; set; }
        
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public bool? Admin { get; set; }
        
        public String Address { get; set; }
        public int? ZipCode { get; set; }

        [DisplayFormat(DataFormatString= "{0:dd/MM/yyyy}", ApplyFormatInEditMode=true)]
        public DateTime? DateOfBirth { get; set; } 

        public ICollection<Order> Orders { get; set; }

        public String Password { get; set; }

        public UserLogin UserLogin { get; set; }

        public String toString()
        {
            return "UserId: " + UserId + " - FirstName : " + FirstName + " - LastName: " + LastName;
        }

    }

}