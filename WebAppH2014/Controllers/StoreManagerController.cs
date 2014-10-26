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
    public class StoreManagerController : Controller
    {
        private SalesItemBLL db = new SalesItemBLL();
        private GenreBLL genreDb = new GenreBLL();
        //
        // GET: /StoreManager/
        public ActionResult Index()
        {

            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            //TODO What does this line do?
            //var items = db.SalesItems.Include(a => a.Genre);
           // return View(items.ToList());
            return View();
        }

        public ViewResult Details(int id)
        {

            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            SalesItem item = db.findSalesItem(id);
            return View(item);
        }

        public ActionResult Create()
        {

            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            ViewBag.GenreId = new SelectList(genreDb.getGenres(), "GenreId", "Name");
            return View();
        }

        //
        // POST: /StoreManager/Create

        [HttpPost]
        public ActionResult Create(SalesItem item)
        {
            item.ImageUrl = "placeholder";
            if (ModelState.IsValid)
            {
                db.addSalesItem(item);
                return RedirectToAction("Index");
            }

            ViewBag.GenreId = new SelectList(genreDb.getGenres(), "GenreId", "Name", item.GenreId);
            return View(item);
        }



        public ActionResult Edit(int id)
        {
            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            SalesItem item = db.findSalesItem(id);
            ViewBag.GenreId = new SelectList(genreDb.getGenres(), "GenreId", "Name", item.GenreId);
            return View(item);
        }

        //
        // POST: /StoreManager/Edit/5

        [HttpPost]
        public ActionResult Edit(SalesItem item)
        {
            if (ModelState.IsValid)
            {
                db.editSalesItem(item);
                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(genreDb.getGenres(), "GenreId", "Name", item.GenreId);
            return View(item);
        }
        public ActionResult Delete(int id)
        {
            SalesItem item = db.findSalesItem(id);
            return View(item);
        }

        //
        // POST: /StoreManager/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesItem item = db.findSalesItem(id);
            db.removeSalesItem(item);
            return RedirectToAction("Index");
        }

        public void Slett(int id)
        {
            // denne kalles via et Ajax-kall
            SalesItem item = db.findSalesItem(id);
            db.removeSalesItem(item);
            // kunne returnert en feil dersom slettingen feilet....
        }

        //Martin hva gjør den her? copy-paste?
      /*  protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
       * */


        private Boolean isAdmin()
        {
            if (isLoggedIn())
            {
                UserBLL userDb = new UserBLL();
                int userId = (int)Session["UserId"];
                User currentUser = userDb.getUser(userId);
                if (currentUser.Admin != null && currentUser.Admin == true)
                    return true;
            }
            
            return false;
        }
        private Boolean isLoggedIn()
        {
            if (Session["LoggedIn"] != null)
            {
                if (ViewBag.isUser != 0)
                    return (bool)Session["LoggedIn"];
                else
                    return false;
            }
            return false;
        }
	}
}