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
            var items = db.SalesItems.Include(a => a.Genre);
            return View(items.ToList());
        }

        public ViewResult Details(int id)
        {
            SalesItem item = db.SalesItems.Find(id);
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
	}
}