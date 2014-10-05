using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppH2014.Models;

namespace WebAppH2014.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
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
        public ActionResult Index(User inUser)
        {
            if (isUserInDB(inUser))
            {
                Debug.WriteLine("Login == true");
                Session["LoggedIn"] = true;
                ViewBag.isLoggedIn = true;
                return View();
            }
            else
            {
                Debug.WriteLine("Login == false");
                Session["LoggedIn"] = false;
                ViewBag.isLoggedIn = false;
                return View();
            }
        }

        private static bool isUserInDB(User inUser)
        {
            Debug.WriteLine(inUser.UserId);
            Debug.WriteLine(inUser.FirstName + " - " + inUser.LastName);
            Debug.WriteLine(inUser.Password);

            using (var db = new StoreContext())
            {

                //these fields are dependent on index.cshtml modelformat used to generate the inUser
                byte[] passordDb = genHash(inUser.Password);
                UserLogin foundUser = db.UserPasswords.Where(b => b.Password == passordDb && b.UserName == inUser.UserLogin.UserName).FirstOrDefault();
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
            using (var db = new StoreContext())
            {
                try
                {
                    var newUser = new User { FirstName = inUser.FirstName, LastName = inUser.LastName };
                    byte[] passwordDb = genHash(inUser.Password);
                    UserLogin userlogin = new UserLogin { UserId = newUser.UserId, Password = passwordDb, UserName = inUser.UserLogin.UserName };
                    newUser.UserLogin = userlogin;
  
                    db.Users.Add(newUser);
                    db.UserPasswords.Add(userlogin);
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
        }
    }
}