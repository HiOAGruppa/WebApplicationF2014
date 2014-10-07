using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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
                //saves userId and loginState
                Session["UserId"] = inUser.UserId;
                Session["LoggedIn"] = true;
                ViewBag.isLoggedIn = true;
                ViewBag.isUserId = inUser.UserId;
                return View();
            }
            else
            {
                Debug.WriteLine("Login == false");
                Session["LoggedIn"] = false;
                ViewBag.isUser = 0;
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
                    inUser.UserId = foundUser.UserId;


                    return false;
                }
                else
                {
                    inUser.UserId = foundUser.UserId;
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
            if (isLoggedIn())
            {
                return View();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inUser">The user recieved by the controller from .cshtml. Can be partyle empty.</param>
        /// <returns>The view</returns>
        public ActionResult ModifyUser(User inUser)
        {
            if (isLoggedIn())
            {
                using (var db = new StoreContext())
                {
                    int userId = (int)Session["UserId"];
                    modifyUserInfo(inUser, db);

                    Debug.WriteLine(userId);

                    try
                    {
                        User currentUser = db.getUser(userId);
                        Debug.WriteLine(currentUser.toString());

                        return View(currentUser);
                        // return RedirectToAction("ModifyUser", db.getUser(userId));
                    }
                    catch
                    {
                        return RedirectToAction("Index");
                    }
                }
            }


            return RedirectToAction("Index");
        }

        //Checks if the user is currently logged in. also checks if userid is valid
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

        private void modifyUserInfo(User user, StoreContext db)
        {
            if (user.UserId != 0)
            {
                User userInDb = db.getUser(user.UserId);
                if (user.FirstName != userInDb.FirstName && user.FirstName != "")
                    userInDb.FirstName = user.FirstName;
                if (user.LastName != userInDb.LastName && user.LastName != "")
                    userInDb.LastName = user.LastName;
                if (user.ZipCode != userInDb.ZipCode)
                    userInDb.ZipCode = user.ZipCode;
                if (user.Address != userInDb.Address)
                    userInDb.Address = user.Address;
                if (user.DateOfBirth != userInDb.DateOfBirth)
                    userInDb.DateOfBirth = user.DateOfBirth;
           //         makeDate(user.DateOfBirth);
                db.SaveChanges();
            }
            Debug.WriteLine("modifyUser: \n" + user.toString());
            return;
        }

    }
}