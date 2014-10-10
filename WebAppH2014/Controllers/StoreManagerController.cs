using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using WebAppH2014.Models;

namespace WebAppH2014.Controllers
{
    public class StoreManagerController : Controller
    {
        private StoreContext db = new StoreContext();
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
            var items = db.SalesItems.Include(a => a.Genre);
            return View(items.ToList());
        }

        public ViewResult Details(int id)
        {

            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            SalesItem item = db.SalesItems.Find(id);
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
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name");
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
                db.SalesItems.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", item.GenreId);
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
            SalesItem item = db.SalesItems.Find(id);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", item.GenreId);
            return View(item);
        }

        //
        // POST: /StoreManager/Edit/5

        [HttpPost]
        public ActionResult Edit(SalesItem item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", item.GenreId);
            return View(item);
        }
        public ActionResult Delete(int id)
        {
            SalesItem item = db.SalesItems.Find(id);
            return View(item);
        }

        //
        // POST: /StoreManager/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesItem item = db.SalesItems.Find(id);
            db.SalesItems.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void Slett(int id)
        {
            // denne kalles via et Ajax-kall
            SalesItem item = db.SalesItems.Find(id);
            db.SalesItems.Remove(item);
            db.SaveChanges();
            // kunne returnert en feil dersom slettingen feilet....
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        private Boolean isAdmin()
        {
            if (isLoggedIn())
            {
                int userId = (int)Session["UserId"];
                User currentUser = db.getUser(userId);
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