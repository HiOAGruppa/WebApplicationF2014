using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppH2014.Models;

namespace WebAppH2014.Controllers
{
    public class DbTestController : Controller
    {

        private StoreContext db = new StoreContext();

        // GET: DbTest
        public ActionResult Index()
        {
            var users = db.Users.ToList();

            //kan bare bruke en modell per page. lag modell"container" eller wtf
  //          var orders = db.Orders.ToList();

            return View(users);
        }
    }
}