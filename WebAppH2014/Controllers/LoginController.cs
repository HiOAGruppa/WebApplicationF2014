using System;
using System.Collections;
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
                    return false;
                }
                else
                {
                    inUser.UserId = foundUser.UserId;
                    return true;
                }
            }
        }

        public ActionResult UserPage()
        {
            StoreContext db = new StoreContext();
            if (isLoggedIn())
            {
                int id = (int)Session["UserId"];
                User user = db.getUser(id);

                if (user == null)
                    HttpNotFound();

                return View(user);
            }

            return RedirectToAction("Index");
        }

        public ActionResult UserOrders()
        {
            StoreContext db = new StoreContext();
            int id = (int)Session["UserId"];
            User currentUser = db.getUser(id);

            if (currentUser == null)
               RedirectToAction("UserPage");

            if (currentUser.Orders != null)
            {
                var ordre = currentUser.Orders.ToList<Order>();
                return View(ordre);
            }
            
            return RedirectToAction("UserPage");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inUser">The user recieved by the controller from .cshtml. Can be partly empty.</param>
        /// <returns>The view</returns>
        public ActionResult ModifyUser(UserModifyUser inUser)
        {

            if (inUser.NewPassword != inUser.ConfirmNewPassword && inUser.NewPassword != null && inUser.ConfirmNewPassword != null)
            {
                ModelState.AddModelError("passwordFormatError", "Your new passwords don't match.");
            }

            if (isLoggedIn())
            {
                using (var db = new StoreContext())
                {
                    int userId = (int)Session["UserId"];
                    UserModifyUser problematicSave = modifyUserInfo(inUser, db);

                    if (problematicSave != null)
                        return View(problematicSave);

                    Debug.WriteLine(userId);

                    try
                    {
                        User currentUser = db.getUser(userId);
                        Debug.WriteLine(currentUser.toString());
                        UserModifyUser editUser = new UserModifyUser(currentUser);
                        return View(editUser);
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
        //returns null if everything went well.
        //returns current UsermodifyUser-object for further editing if we didnt save info properly
        private UserModifyUser modifyUserInfo(UserModifyUser user, StoreContext db)
        {
            if (user.UserId != 0)
            {
                //changes all user-settings that differ from db-object
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



                bool passwordMatchesHash = StructuralComparisons.StructuralEqualityComparer.Equals(genHash(user.OldPassword), userInDb.UserLogin.Password);
                //validates that new password has been typed twice, and that old password matches old hashed password.
                if (user.NewPassword == user.ConfirmNewPassword && passwordMatchesHash)
                {
                    Debug.WriteLine("If-clause has been triggered for pw-save");
                    userInDb.UserLogin.Password = genHash(user.NewPassword);
                }

                //tells user no settings will be saved if old password is mistyped
                if (!passwordMatchesHash)
            {
                ModelState.AddModelError("oldPasswordIncorrect", "Du har skrevet feil passord, ingen endringer vil bli lagret.");
                return user;
            }
            else
            {
                Debug.WriteLine("settings saved");
                db.SaveChanges();
            }
            }
            Debug.WriteLine("modifyUser: \n" + user.toString());
            return null;
        }

    }
}