using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppH2014.Models
{
    public class UserLogin
    {

        [Key]
        public int UserId { get; set; }
        [StringLength(450)]
        [Index("UserNameIndex", IsUnique = true)]
        [Required(ErrorMessage = "Brukernavn må oppgis")]
        public String UserName { get; set; }
        public byte[] Password { get; set; }

    }
}