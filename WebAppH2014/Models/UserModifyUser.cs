using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppH2014.Models
{
    /// <summary>
    /// The class is used for handling the information in both login and registry of users.
    /// Particularly to separate out the handling of passwords in cleartext to a temporary object.
    /// </summary>
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