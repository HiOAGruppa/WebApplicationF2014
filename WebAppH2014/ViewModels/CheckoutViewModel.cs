using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppH2014.ViewModels
{
    public class CheckoutViewModel
    {
        public int PersonId { get; set; }
        [Required(ErrorMessage = "Fornavn må oppgis")]
        [StringLength(30, ErrorMessage = "Maks 30 tegn i fornavn")]
        public String Firstname { get; set; }

        [Required(ErrorMessage = "Etternavn må oppgis")]
        [StringLength(50, ErrorMessage = "Maks 50 tegn i etternavn")]
        public String Lastname { get; set; }

        [Required(ErrorMessage = "Adresse må oppgis")]
        public String Address { get; set; }

        [Required(ErrorMessage = "Postnummer må oppgis")]
        public int? Zipcode{get;set;}
    }
}