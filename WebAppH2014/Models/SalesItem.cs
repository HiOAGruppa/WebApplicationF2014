using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppH2014.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebAppH2014.Models
{
    public class SalesItem
    {
        //TODO Complete class in accordance with what DB+View needs.
        public int SalesItemId { get; set; }

        [Required(ErrorMessage = "Pris må oppgis")]
        [Range(0.00, 100000.00, ErrorMessage = "Pris må være mellom 0.00 og 100 000.00")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Navn må oppgis")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Må gi en beskrivelse")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Må oppgi antall på lager")]
        public int InStock { get; set; }
        [Required(ErrorMessage = "Kategori må oppgis")]
        public int? GenreId { get; set; }
        public string ImageUrl { get; set; }

        [DisplayName("Kategori")]
        public Genre Genre { get; set; }
        public ICollection<OrderSalesItem> IncludedInOrders { get; set; }



    }
}