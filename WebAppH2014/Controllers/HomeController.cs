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
            var salesItems = db.SalesItems.ToList();
            return View(salesItems);
        }

        public ActionResult Item()
        {
            return View();
        }

        public ActionResult SalesItemView()
        {
            return View();
        }
    }
}