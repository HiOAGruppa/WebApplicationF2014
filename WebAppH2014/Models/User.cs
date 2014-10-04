using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebAppH2014.Models
{
    public class User
    {
         public int UserId { get; set; }
        /*
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Address { get; set; }
        public int ZipCode { get; set; }

        public DateTime DateOfBirth { get; set; } */

        public ICollection<Order> Orders { get; set; }

        public String UserName { get; set; }
        public String Password { get; set; }
    }

    public class dbUser
    {
        [Key]
        public String UserName { get; set; }
        public byte[] Password { get; set; }
    }

    public class UserContext : DbContext
    {
        public UserContext()
            : base("name=User")
        {
            Database.CreateIfNotExists();
        }

        public DbSet<dbUser> Users { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}