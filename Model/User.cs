using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model
{
    public class User
    {

         public int UserId { get; set; }
         public bool? Admin { get; set; }

        [Required(ErrorMessage = "Fornavn må oppgis")]
        [StringLength(30, ErrorMessage = "Maks 30 tegn i fornavn")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Etternavn må oppgis")]
        [StringLength(50, ErrorMessage = "Maks 50 tegn i etternavn")]
        public String LastName { get; set; }

        public String Address { get; set; }
       
        
        public int? ZipCode { get; set; }

        [DisplayFormat(DataFormatString= "{0:MM/dd/yyyy}", ApplyFormatInEditMode=true)]
        public DateTime? DateOfBirth { get; set; } 

        public ICollection<Order> Orders { get; set; }

        public UserLogin UserLogin { get; set; }

        public String toString()
        {
            return "UserId: " + UserId + " - FirstName : " + FirstName + " - LastName: " + LastName;
        }

    }

}