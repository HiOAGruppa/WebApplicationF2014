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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (Session["LoggedIn"] == null)
            {
                Session["LoggedIn"] = false;
                ViewBag.isLoggedIn = false;
            }
            else
            {
                ViewBag.isLoggedIn = (bool)Session["LoggedIn"];
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(User inUser)
        {
            if (isUserInDB(inUser))
            {
                Session["LoggedIn"] = true;
                ViewBag.isLoggedIn = true;
                return View();
            }
            else
            {
                Session["LoggedIn"] = false;
                ViewBag.isLoggedIn = false;
                return View();
            }
        }

        private static bool isUserInDB(User inUser)
        {
            using (var db = new UserContext())
            {
                byte[] passordDb = genHash(inUser.Password);
                dbUser foundUser = db.Users.FirstOrDefault(b => b.Password == passordDb && b.UserName == inUser.UserName);
                if (foundUser == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User inUser)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            using (var db = new UserContext())
            {
                try
                {
                    var newUser = new dbUser();
                    byte[] passwordDb = genHash(inUser.Password);
                    newUser.Password = passwordDb;
                    newUser.UserName = inUser.UserName;
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
        }

        private static byte[] genHash(string inPassword)
        {
            byte[] inData, outData;
            var algorithm = System.Security.Cryptography.SHA256.Create();
            inData = System.Text.Encoding.ASCII.GetBytes(inPassword);
            outData = algorithm.ComputeHash(inData);
            return outData;
        }

        // FOR TESTVIEW (LoggedIn.cshtml)
        public ActionResult LoggedIn()
        {
            if (Session["LoggedIn"] != null)
            {
                bool loggedIn = (bool)Session["LoggedIn"];
                if (loggedIn)
                {
                    return View();
                }
            }
            return RedirectToAction("Index");
        }        // UNUSED??? BAD TODO-PRACTICE #YOLO
        /*     
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        */
    }
}