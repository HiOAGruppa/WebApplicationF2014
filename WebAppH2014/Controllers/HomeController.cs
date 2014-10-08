using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppH2014.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private StoreContext db = new StoreContext();

        public ActionResult Index()
        {

            var genreModel = db.Genres.Include("Items")
                .Single(g => g.Name == "Data");

            return View(genreModel);
            //return View();
        }

        public ActionResult Item()
        {
            return View();
        }

        // UNUSED??? BAD TODO-PRACTICE #YOLO
        /*     
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        */
    }
}