using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;

namespace WebAppH2014.Controllers
{
    public class StoreController : Controller
    {
        SalesItemBLL itemDb = new SalesItemBLL();
        GenreBLL genreDb = new GenreBLL();

        //
        // GET: /Store/
        public ActionResult Index()
        {
            var genres = genreDb.getGenres();

            return View(genres);
        }

        public ActionResult Details(int id = 0)
        {
            SalesItem item = itemDb.findSalesItem(id);

            if (item == null)
                HttpNotFound();

            return View(item);
        }

        public ActionResult Browse(string genre)
        {
            //TODO IMPLEMENT MARTIN
            // Retrieve Genre and its Associated Albums from database
          //  var genreModel = db.Genres.Include("Items")
          //      .Single(g => g.Name == genre);

           // return View(genreModel);
            return View();
        }
        public ActionResult GenreMenu()
        {
            var genres = genreDb.getGenres();

            return PartialView(genres);
        }
	}
}