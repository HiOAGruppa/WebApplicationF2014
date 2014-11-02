using BLL;
using Model;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace WebAppH2014.Controllers
{
    public class StoreManagerController : Controller
    {
        private ISalesItemBLL _itemBLL;
        private IUserBLL _userBLL;
        private IOrderBLL _orderBLL;
        private IGenreBLL _genreBLL;

        public StoreManagerController()
        {
            _itemBLL = new SalesItemBLL();
            _userBLL = new UserBLL();
            _orderBLL = new OrderBLL();
            _genreBLL = new GenreBLL();
        }
        public StoreManagerController(ISalesItemBLL itemStub, IUserBLL userStub, IOrderBLL orderStub, IGenreBLL genreStub)
        {
            _itemBLL = itemStub;
            _userBLL = userStub;
            _orderBLL = orderStub;
            _genreBLL = genreStub;
        }

        //
        // GET: /StoreManager/
        public ActionResult Index()
        {
            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            //TODO What does this line do?
            var items = _itemBLL.getSalesItemsWithGenre();
            return View(items);
        }

        public ViewResult Details(int id)
        {
            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            SalesItem item = _itemBLL.findSalesItem(id);
            return View(item);
        }

        public ActionResult Create()
        {
            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            ViewBag.GenreId = new SelectList(_genreBLL.getGenres(), "GenreId", "Name");
            return View();
        }

        //
        // POST: /StoreManager/Create

        [HttpPost]
        public ActionResult Create(SalesItem item)
        {
            item.ImageUrl = "placeholder";
            if (ModelState.IsValid)
            {
                var ok = _itemBLL.addSalesItem(item);
                if (ok)
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.GenreId = new SelectList(_genreBLL.getGenres(), "GenreId", "Name", item.GenreId);
            return View(item);
        }



        public ActionResult Edit(int id)
        {
            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            SalesItem item = _itemBLL.findSalesItem(id);
            ViewBag.GenreId = new SelectList(_genreBLL.getGenres(), "GenreId", "Name", item.GenreId);
            return View(item);
        }

        //
        // POST: /StoreManager/Edit/5

        [HttpPost]
        public ActionResult Edit(SalesItem item)
        {
            if (ModelState.IsValid)
            {
                var ok =_itemBLL.editSalesItem(item);
                if(ok)
                    return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(_genreBLL.getGenres(), "GenreId", "Name", item.GenreId);
            return View(item);
        }


        //
        // POST: /StoreManager/Delete/5

        public void Slett(int id)
        {
            // denne kalles via et Ajax-kall
            SalesItem item = _itemBLL.findSalesItem(id);
            var ok = _itemBLL.removeSalesItem(item);
            // kunne returnert en feil dersom slettingen feilet....
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(SalesItem item)
        {
            var ok = _itemBLL.removeSalesItem(item);
            if (ok)
                return RedirectToAction("Index"); 
            else
                return View();

        }

        public ActionResult Kunder()
        {
            var users = _userBLL.getUsers();
            
            return View(users);
        }

        public void SlettUser(int id)
        {
            // denne kalles via et Ajax-kall
            User user = _userBLL.getUser(id);
            if (user.Admin != true)
            {
                var ok = _userBLL.removeUser(user);
            }
            // kunne returnert en feil dersom slettingen feilet....
        }

        public ActionResult EditUser(int id)
        {
            if (!isAdmin())
            {
                string error = "Du har ikke rettigheter til dette!";
                ViewBag.ErrorMessage = error;
                return View("Error");
            }
            User user = _userBLL.getUser(id);
            UserModifyUser displayUser = new UserModifyUser(user);
            return View(displayUser);
        }

        //
        // POST: /StoreManager/Edit/5

        [HttpPost]
        public ActionResult EditUser(UserModifyUser user)
        {
            if (ModelState.IsValid)
            {
                UserModifyUser problematicSave = modifyUserInfo(user);

                //if returned null, method executed without fault
                if (problematicSave != null)
                    return View(problematicSave);

                try
                {
                    User currentUser = _userBLL.getUser(user.UserId);
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
            var orders = _orderBLL.getOrders();
            return View(orders);
        }
        public void SlettOrder(int id)
        {
            Debug.Print("Order id: " + id);
            // denne kalles via et Ajax-kall
            _orderBLL.removeOrder(id);
            // kunne returnert en feil dersom slettingen feilet....
        }
        private Boolean isAdmin()
        {
            //FOR TESTING
            return true;
            /*if (isLoggedIn())
            {
                int userId = (int)Session["UserId"];
                User currentUser = _userBLL.getUser(userId);
                if (currentUser.Admin != null && currentUser.Admin == true)
                    return true;
            }
            
            return false;*/
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
            User userInDb = _userBLL.getUser(user.UserId);

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

            var ok = _userBLL.editUser(userInDb.UserId, userInDb);

            return null;
        }
	}
}