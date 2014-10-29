using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using System.Diagnostics;

namespace WebAppH2014.Controllers
{
    public class StoreManagerController : Controller
    {
        //
        // GET: /StoreManager/
        public ActionResult Index()
        {
            SalesItemBLL db = new SalesItemBLL();
            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            //TODO What does this line do?
            var items = db.getSalesItemsWithGenre();
            return View(items);
        }

        public ViewResult Details(int id)
        {
            SalesItemBLL db = new SalesItemBLL();
            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            SalesItem item = db.findSalesItem(id);
            return View(item);
        }

        public ActionResult Create()
        {
            GenreBLL genreDb = new GenreBLL();
            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            ViewBag.GenreId = new SelectList(genreDb.getGenres(), "GenreId", "Name");
            return View();
        }

        //
        // POST: /StoreManager/Create

        [HttpPost]
        public ActionResult Create(SalesItem item)
        {
            GenreBLL genreDb = new GenreBLL();
            SalesItemBLL db = new SalesItemBLL();
            item.ImageUrl = "placeholder";
            if (ModelState.IsValid)
            {
                db.addSalesItem(item);
                return RedirectToAction("Index");
            }

            ViewBag.GenreId = new SelectList(genreDb.getGenres(), "GenreId", "Name", item.GenreId);
            return View(item);
        }



        public ActionResult Edit(int id)
        {
            GenreBLL genreDb = new GenreBLL();
            SalesItemBLL db = new SalesItemBLL();
            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            SalesItem item = db.findSalesItem(id);
            ViewBag.GenreId = new SelectList(genreDb.getGenres(), "GenreId", "Name", item.GenreId);
            return View(item);
        }

        //
        // POST: /StoreManager/Edit/5

        [HttpPost]
        public ActionResult Edit(SalesItem item)
        {
            GenreBLL genreDb = new GenreBLL();
            SalesItemBLL db = new SalesItemBLL();
            if (ModelState.IsValid)
            {
                db.editSalesItem(item);
                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(genreDb.getGenres(), "GenreId", "Name", item.GenreId);
            return View(item);
        }
        public ActionResult Delete(int id)
        {
            SalesItemBLL db = new SalesItemBLL();
            SalesItem item = db.findSalesItem(id);
            return View(item);
        }

        //
        // POST: /StoreManager/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesItemBLL db = new SalesItemBLL();
            SalesItem item = db.findSalesItem(id);
            db.removeSalesItem(item);
            return RedirectToAction("Index");
        }

        public void Slett(int id)
        {
            SalesItemBLL db = new SalesItemBLL();
            // denne kalles via et Ajax-kall
            SalesItem item = db.findSalesItem(id);
            db.removeSalesItem(item);
            // kunne returnert en feil dersom slettingen feilet....
        }

        public ActionResult Kunder()
        {
            UserBLL userBll = new UserBLL();
            var users = userBll.getUsers();
            
            return View(users);
        }

        public void SlettUser(int id)
        {
            UserBLL db = new UserBLL();
            // denne kalles via et Ajax-kall
            User user = db.getUser(id);
            db.removeUser(user);
            // kunne returnert en feil dersom slettingen feilet....
        }

        public ActionResult EditUser(int id)
        {
            UserBLL db = new UserBLL();
            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            User user = db.getUser(id);
            UserModifyUser displayUser = new UserModifyUser(user);
            return View(displayUser);
        }

        //
        // POST: /StoreManager/Edit/5

        [HttpPost]
        public ActionResult EditUser(UserModifyUser user)
        {
            UserBLL db = new UserBLL();
            if (ModelState.IsValid)
            {
                UserModifyUser problematicSave = modifyUserInfo(user);

                //if returned null, method executed without fault
                if (problematicSave != null)
                    return View(problematicSave);

                try
                {
                    User currentUser = db.getUser(user.UserId);
                    Debug.WriteLine(currentUser.toString());
                    UserModifyUser editUser = new UserModifyUser(currentUser);
                    //return View(editUser);
                    return RedirectToAction("Kunder");
                }
                catch
                {
                    return RedirectToAction("Kunder");
                }
            }
            return RedirectToAction("Kunder");
        }
        public ActionResult Ordre()
        {
            OrderBLL db = new OrderBLL();
            var orders = db.getOrders();
            /*foreach (Order o in orders)
            {
                Debug.Print("Order: " + o.OrderId);
                foreach (OrderSalesItem i in o.SalesItems)
                    Debug.Print("Item: " + i.SalesItem.Name);
            }*/
            return View(orders);
        }
        public void SlettOrder(int id)
        {
            OrderBLL db = new OrderBLL();
            Debug.Print("Order id: " + id);
            // denne kalles via et Ajax-kall
            db.removeOrder(id);
            // kunne returnert en feil dersom slettingen feilet....
        }
        private Boolean isAdmin()
        {
            if (isLoggedIn())
            {
                UserBLL userDb = new UserBLL();
                int userId = (int)Session["UserId"];
                User currentUser = userDb.getUser(userId);
                if (currentUser.Admin != null && currentUser.Admin == true)
                    return true;
            }
            
            return false;
        }
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


        private UserModifyUser modifyUserInfo(UserModifyUser user)
        {
            UserBLL db = new UserBLL();
            User userInDb = db.getUser(user.UserId);

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

            db.editUser(userInDb.UserId, userInDb);

            return null;
        }
	}
}