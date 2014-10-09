using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppH2014.Models
{
    public class UserModifyUser : User
    {
        public UserModifyUser()
        {

        }
        public UserModifyUser(User user) : base()
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserId = user.UserId;
            ZipCode = user.ZipCode;
            Address = user.Address;
            DateOfBirth = user.DateOfBirth;
        }

        public String NewPassword { get; set; }
        public String OldPassword { get; set; }
        public String ConfirmNewPassword { get; set; }


    }
}