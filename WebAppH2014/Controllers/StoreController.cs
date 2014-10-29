using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using System.Diagnostics;

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
            var genres = genreDb.getGenres().ToList();

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
            var genreModel = genreDb.getSelectedGenre(genre);
            return View(genreModel);
        }
        public ActionResult GenreMenu()
        {
            var genres = genreDb.getGenres();

            return PartialView(genres);
        }
	}
}