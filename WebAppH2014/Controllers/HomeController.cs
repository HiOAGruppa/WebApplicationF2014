using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;


namespace WebAppH2014.Controllers
{
    public class HomeController : Controller
    {
        SalesItemBLL db = new SalesItemBLL();

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