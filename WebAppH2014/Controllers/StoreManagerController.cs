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
	}
}