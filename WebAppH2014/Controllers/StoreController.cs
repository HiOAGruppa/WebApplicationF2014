using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using WebAppH2014.Models;

namespace WebAppH2014.Controllers
{
    public class StoreController : Controller
    {

        private StoreContext db = new StoreContext();
        //
        // GET: /Store/
        public ActionResult Index()
        {
            var genres = db.Genres.ToList();

            return View(genres);
        }

        public ActionResult Details(int id = 0)
        {
            SalesItem item = db.SalesItems.Find(id);

            if (item == null)
                HttpNotFound();

            return View(item);
        }

        public ActionResult Browse(string genre)
        {
            // Retrieve Genre and its Associated Albums from database
            var genreModel = db.Genres.Include("Items")
                .Single(g => g.Name == genre);

            return View(genreModel);
        }
        public ActionResult GenreMenu()
        {
            var genres = db.Genres.ToList();

            return PartialView(genres);
        }
	}
}