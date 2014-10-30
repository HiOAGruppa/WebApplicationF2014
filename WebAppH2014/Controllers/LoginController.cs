using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;

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
            ViewBag.WrongPassword = false;
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserModifyUser inUser)
        {
            UserBLL db = new UserBLL();
            if (db.isUserInDB(inUser))
            {
                Debug.WriteLine("Login == true");
                LogIn(inUser);
                ViewBag.WrongPassword = false;
                return View();
            }
            else
            {
                Debug.WriteLine("Login == false");
                LogOut();
                ViewBag.WrongPassword = true;
                return View();
            }
        }


        public ActionResult UserPage()
        {
            UserBLL db = new UserBLL();
            if (isLoggedIn())
            {
                int id = (int)Session["UserId"];
                User user = db.getUser(id);

                if (user == null)
                    HttpNotFound();
                if (user.Admin != null && user.Admin == true)
                    return RedirectToAction("AdminView");
                else
                    return View(user);
            }

            return RedirectToAction("Index");
        }

        public ActionResult AdminView()
        {
            return View();
        }

        public ActionResult UserOrders()
        {
            UserBLL db = new UserBLL();
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
        public ActionResult Register(UserModifyUser inUser)
        {
            UserBLL db = new UserBLL();
            if (!ModelState.IsValid)
            {
                return View();
            }

                try
                {
                    var newUser = new User { FirstName = inUser.FirstName, LastName = inUser.LastName, Address = inUser.Address, ZipCode = inUser.ZipCode};
                    byte[] passwordDb = genHash(inUser.OldPassword);
                    UserLogin userlogin = new UserLogin { UserId = newUser.UserId, Password = passwordDb, UserName = inUser.UserLogin.UserName };
                    newUser.UserLogin = userlogin;

                    db.addUser(newUser, userlogin);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
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

        public ActionResult ModifyUser()
        {
            UserBLL db = new UserBLL();
            if (isLoggedIn())
            {
                User userInDb = db.getUser((int)Session["UserId"]);
                UserModifyUser displayUser = new UserModifyUser(userInDb);
                return View(displayUser);
            }
            return RedirectToAction("index");
        
        }
        [HttpPost]
        public ActionResult ModifyUser(UserModifyUser inUser)
        {

            if (inUser.NewPassword != inUser.ConfirmNewPassword && inUser.NewPassword != null && inUser.ConfirmNewPassword != null)
            {
                ModelState.AddModelError("passwordFormatError", "Your new passwords don't match.");
                return View(inUser);
            }

            if (isLoggedIn())
            {
                UserBLL db = new UserBLL();
                    int userId = (int)Session["UserId"];

                    UserModifyUser problematicSave = modifyUserInfo(inUser);

                    //if returned null, method executed without fault
                    if (problematicSave != null)
                        return View(problematicSave);

                    try
                    {
                        User currentUser = db.getUser(userId);
                        Debug.WriteLine(currentUser.toString());
                        UserModifyUser editUser = new UserModifyUser(currentUser);
                        //return View(editUser);
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return RedirectToAction("Index");
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

        //Renders the Login button to reflect whether the user is logged in or not. (shows log in/my page)
        [ChildActionOnly]
        public ActionResult LoginLink()
        {
            if (isLoggedIn())
            {
                ViewData["LoginText"] = "Min Side";
                ViewData["LoginLink"] = "UserPage";
            }
            else {
                ViewData["LoginText"] = "Logg inn";
                ViewData["LoginLink"] = "Index";

            }
            return PartialView("LoginNav");
        }

        //returns null if everything went well.
        //returns current UsermodifyUser-object for further editing if we didnt save info properly
        private UserModifyUser modifyUserInfo(UserModifyUser user)
        {
            UserBLL db = new UserBLL();
            int userId = (int)Session["UserId"];
            if (userId != 0)
            {
                User userInDb = db.getUser(userId);

                //did user type in correct password compared to database entry?
                bool passwordMatchesHash = false;
                if (user.OldPassword != null)
                    passwordMatchesHash = StructuralComparisons.StructuralEqualityComparer.Equals(genHash(user.OldPassword), userInDb.UserLogin.Password);

                //tells user no settings will be saved if old password is mistyped or null
                if (!passwordMatchesHash || user.OldPassword == null)
                {
                    ModelState.AddModelError("oldPasswordIncorrect", "Du har skrevet feil passord, ingen endringer vil bli lagret.");
                    return user;
                }

                //changes all user-settings that differ from db-object
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

                //validates that new password has been typed twice, and that old password matches old hashed password.
                if (user.NewPassword == user.ConfirmNewPassword && passwordMatchesHash && user.NewPassword != null)
                {
                    Debug.WriteLine("If-clause has been triggered for pw-save");
                    userInDb.UserLogin.Password = genHash(user.NewPassword);
                }

                Debug.WriteLine("settings saved");
                db.editUser(userInDb.UserId, userInDb);
                Debug.WriteLine("modifyUser: \n" + user.toString());
            }
            else
            {
                return user;
            }
           
            return null;
        }

        //method for defining user as logged in.
        private void LogIn(User inUser)
        {
            //saves userId and loginState
            Session["UserId"] = inUser.UserId;
            Session["LoggedIn"] = true;
            ViewBag.isLoggedIn = true;
            ViewBag.isUserId = inUser.UserId;
           
        }
        //method for defining a user as logged out.
        private void LogOut()
        {
            //saves userId and loginState
            Session["UserId"] = 0;
            Session["LoggedIn"] = false;
            ViewBag.isLoggedIn = false;
            ViewBag.isUserId = 0;
        }

        public ActionResult LogoutAction()
        {
            LogOut();
            return RedirectToAction("index");
        }

    }
}