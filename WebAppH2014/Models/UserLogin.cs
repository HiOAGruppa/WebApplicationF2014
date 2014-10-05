using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppH2014.Models
{
    public class UserLogin
    {

        [Key]
        public int UserId { get; set; }
        public String UserName { get; set; }
        public byte[] Password { get; set; }

    }
}