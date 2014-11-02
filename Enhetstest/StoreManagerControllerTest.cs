using BLL;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppH2014.Controllers;

namespace Enhetstest
{
    [TestClass]
    public class StoreManagerControllerTest
    {

        /*
         * Her tester vi metoder som har med storemanagercontroller (Altså Admindelen) å gjøre.
         */

        /*[SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest(null, "http://tempuri.org", null),
                new HttpResponse(null));
        }
        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }*/

        
        //************************SALES ITEM TESTS****************************
        [TestMethod]
        public void show_alle_items()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var forventetResultat = new List<SalesItem>();
            var item = new SalesItem()
            {
                SalesItemId = 1,
                Price = new decimal(100.0),
                Name = "Keyboard",
                Description = "Fint keyboard",
                InStock = 10,
                GenreId = 1,
                ImageUrl = "bilde",
                Genre = new Genre()
            };
            forventetResultat.Add(item);
            forventetResultat.Add(item);
            forventetResultat.Add(item);

            //Act
            
            var resultat = (ViewResult)controller.Index();
            var resultatListe = (List<SalesItem>)resultat.Model;

            //Assert

            Assert.AreEqual(resultat.ViewName, "");

            for (var i = 0; i < resultatListe.Count; i++)
            {
                Assert.AreEqual(forventetResultat[i].SalesItemId, resultatListe[i].SalesItemId);
                Assert.AreEqual(forventetResultat[i].Price, resultatListe[i].Price);
                Assert.AreEqual(forventetResultat[i].Name, resultatListe[i].Name);
                Assert.AreEqual(forventetResultat[i].Description, resultatListe[i].Description);
                Assert.AreEqual(forventetResultat[i].InStock, resultatListe[i].InStock);
                Assert.AreEqual(forventetResultat[i].GenreId, resultatListe[i].GenreId);
                Assert.AreEqual(forventetResultat[i].ImageUrl, resultatListe[i].ImageUrl);
            }
        }

        [TestMethod]
        public void show_item_view()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var forventetResultat = new SalesItem()
            {
                SalesItemId = 1,
                Price = new decimal(100.0),
                Name = "Keyboard",
                Description = "Fint keyboard",
                InStock = 10,
                GenreId = 1,
                ImageUrl = "bilde",
                Genre = new Genre()
            };

            //Act

            var resultat = (ViewResult)controller.Details(1);
            var resultatItem = (SalesItem)resultat.Model;

            //Assert
            Assert.AreEqual(resultat.ViewName, "");

            Assert.AreEqual(forventetResultat.SalesItemId, resultatItem.SalesItemId);
            Assert.AreEqual(forventetResultat.Price, resultatItem.Price);
            Assert.AreEqual(forventetResultat.Name, resultatItem.Name);
            Assert.AreEqual(forventetResultat.Description, resultatItem.Description);
            Assert.AreEqual(forventetResultat.InStock, resultatItem.InStock);
            Assert.AreEqual(forventetResultat.GenreId, resultatItem.GenreId);
            Assert.AreEqual(forventetResultat.ImageUrl, resultatItem.ImageUrl);
        }

        [TestMethod]
        public void show_create_view()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            //Act
            var resultat = (ViewResult)controller.Create();

            //Assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void create_item_OK()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var inItem = new SalesItem()
            {
                SalesItemId = 1,
                Price = new decimal(100.0),
                Name = "Keyboard",
                Description = "Fint keyboard",
                InStock = 10,
                GenreId = 1,
                ImageUrl = "bilde"
            };
            //Act
            var resultat = (RedirectToRouteResult)controller.Create(inItem);

            //Assert
            Assert.AreEqual(resultat.RouteName, "");
            Assert.AreEqual(resultat.RouteValues.Values.First(), "Index");

        }

        [TestMethod]
        public void create_item_validate_error()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var inItem = new SalesItem();
            controller.ViewData.ModelState.AddModelError("Name", "No Name Given ");

            //Act
            var resultat = (ViewResult)controller.Create(inItem);

            //Assert
            Assert.IsTrue(resultat.ViewData.ModelState.Count == 1);
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void create_item_post_error()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));
            
            var inItem = new SalesItem();
            inItem.Name = "";

            //Act
            var resultat = (ViewResult)controller.Create(inItem);

            //Assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void show_edit_item_view()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var forventetResultat = new SalesItem()
            {
                SalesItemId = 1,
                Price = new decimal(100.0),
                Name = "Keyboard",
                Description = "Fint keyboard",
                InStock = 10,
                GenreId = 1,
                ImageUrl = "bilde",
                Genre = new Genre()
            };

            //Act

            var resultat = (ViewResult)controller.Edit(1);
            var resultatItem = (SalesItem)resultat.Model;

            //Assert
            Assert.AreEqual(resultat.ViewName, "");

            Assert.AreEqual(forventetResultat.SalesItemId, resultatItem.SalesItemId);
            Assert.AreEqual(forventetResultat.Price, resultatItem.Price);
            Assert.AreEqual(forventetResultat.Name, resultatItem.Name);
            Assert.AreEqual(forventetResultat.Description, resultatItem.Description);
            Assert.AreEqual(forventetResultat.InStock, resultatItem.InStock);
            Assert.AreEqual(forventetResultat.GenreId, resultatItem.GenreId);
            Assert.AreEqual(forventetResultat.ImageUrl, resultatItem.ImageUrl);
        }

        [TestMethod]
        public void edit_item_not_found()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var inItem = new SalesItem();
            inItem.SalesItemId = 0;

            //Act
            var resultat = (ViewResult)controller.Edit(inItem);

            //Assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void edit_item_validate_error()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var inItem = new SalesItem();
            controller.ViewData.ModelState.AddModelError("Error", "No Name Given");

            //Act
            var resultat = (ViewResult)controller.Edit(inItem);

            //Assert
            Assert.IsTrue(resultat.ViewData.ModelState.Count == 1);
            Assert.AreEqual(resultat.ViewData.ModelState["Error"].Errors[0].ErrorMessage, "No Name Given");
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void edit_item_OK()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var inItem = new SalesItem()
            {
                SalesItemId = 1,
                Price = new decimal(100.0),
                Name = "Keyboard",
                Description = "Fint keyboard",
                InStock = 10,
                GenreId = 1,
                ImageUrl = "bilde"
            };

            //Act
            var resultat = (RedirectToRouteResult)controller.Edit(inItem);

            //Assert
            Assert.AreEqual(resultat.RouteName, "");
            Assert.AreEqual(resultat.RouteValues.Values.First(), "Index");
        }

        [TestMethod]
        public void edit_item_post_error()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var inItem = new SalesItem();
            inItem.Name = "";

            //Act
            var resultat = (ViewResult)controller.Edit(inItem);

            //Assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void delete_item_OK()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

           var inItem = new SalesItem()
            {
                SalesItemId = 1,
                Price = new decimal(100.0),
                Name = "Keyboard",
                Description = "Fint keyboard",
                InStock = 10,
                GenreId = 1,
                ImageUrl = "bilde",
                Genre = new Genre()
            };

            //Act
            var resultat = (bool)controller.Slett(inItem.SalesItemId);

            //Assert
            Assert.IsTrue(resultat);
        }

         [TestMethod]
        public void delete_item_not_found()
        {
            // Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var inItem = new SalesItem()
            {
                SalesItemId = 0
            };

            // Act
            var resultat = (bool)controller.Slett(inItem.SalesItemId);

            // Assert
            Assert.IsFalse(resultat);
        }


        //************************USER TESTS****************************
        [TestMethod]
        public void show_alle_kunder()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var forventetResultat = new List<User>();
            var user = new User()
            {
                UserId = 1,
                FirstName = "Jon",
                LastName = "Johnsen",
                Admin = false,
                Address = "HeiVeien 1",
                ZipCode = 3412,
                DateOfBirth = new System.DateTime(1991, 1, 1)
            };
            forventetResultat.Add(user);
            forventetResultat.Add(user);
            forventetResultat.Add(user);

            //Act

            var resultat = (ViewResult)controller.Kunder();
            var resultatListe = (List<User>)resultat.Model;

            //Assert

            Assert.AreEqual(resultat.ViewName, "");

            for (var i = 0; i < resultatListe.Count; i++)
            {
                Assert.AreEqual(forventetResultat[i].UserId, resultatListe[i].UserId);
                Assert.AreEqual(forventetResultat[i].FirstName, resultatListe[i].FirstName);
                Assert.AreEqual(forventetResultat[i].LastName, resultatListe[i].LastName);
                Assert.AreEqual(forventetResultat[i].Admin, resultatListe[i].Admin);
                Assert.AreEqual(forventetResultat[i].Address, resultatListe[i].Address);
                Assert.AreEqual(forventetResultat[i].ZipCode, resultatListe[i].ZipCode);
                Assert.AreEqual(forventetResultat[i].DateOfBirth, resultatListe[i].DateOfBirth);
            }
        }

        [TestMethod]
        public void show_edit_user_view()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var forventetResultat = new User()
            {
                UserId = 1,
                FirstName = "Jon",
                LastName = "Johnsen",
                Address = "HeiVeien 1",
                ZipCode = 3412,
                DateOfBirth = new System.DateTime(1991, 1, 1)
            };

            //Act

            var resultat = (ViewResult)controller.EditUser(1);
            var resultatItem = (User)resultat.Model;

            //Assert
            Assert.AreEqual(resultat.ViewName, "");

            Assert.AreEqual(forventetResultat.UserId, resultatItem.UserId);
            Assert.AreEqual(forventetResultat.FirstName, resultatItem.FirstName);
            Assert.AreEqual(forventetResultat.LastName, resultatItem.LastName);
            Assert.AreEqual(forventetResultat.Address, resultatItem.Address);
            Assert.AreEqual(forventetResultat.ZipCode, resultatItem.ZipCode);
            Assert.AreEqual(forventetResultat.DateOfBirth, resultatItem.DateOfBirth);
        }
        [TestMethod]
        public void edit_user_not_found()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var inUser = new User();
            inUser.UserId = 0;

            //Act
            var resultat = (ViewResult)controller.EditUser(inUser.UserId);

            //Assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void edit_user_validate_error()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var inUser = new User();
            controller.ViewData.ModelState.AddModelError("Error", "No id given");

            //Act
            var resultat = (ViewResult)controller.EditUser(inUser.UserId);

            //Assert
            Assert.IsTrue(resultat.ViewData.ModelState.Count == 1);
            Assert.AreEqual(resultat.ViewData.ModelState["Error"].Errors[0].ErrorMessage, "No id given");
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void edit_user_OK()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var inUser = new User()
            {
                UserId = 1,
                FirstName = "Jon",
                LastName = "Johnsen",
                Admin = false,
                Address = "HeiVeien 1",
                ZipCode = 3412,
                DateOfBirth = new System.DateTime(1991, 1, 1)
            }; 

            //Act
            var resultat = (ViewResult)controller.EditUser(inUser.UserId);

            //Assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void delete_user_OK()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var inUser = new User()
            {
                UserId = 1,
                FirstName = "Jon",
                LastName = "Johnsen",
                Admin = false,
                Address = "HeiVeien 1",
                ZipCode = 3412,
                DateOfBirth = new System.DateTime(1991, 1, 1)
            }; 

            //Act
            var resultat = (bool)controller.SlettUser(inUser);

            //Assert
            Assert.IsTrue(resultat);
        }

        [TestMethod]
        public void delete_user_not_found()
        {
            // Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var inUser = new User()
            {
                 UserId = 0
            };

            // Act
            var resultat = (bool)controller.SlettUser(inUser);

            // Assert
            Assert.IsFalse(resultat);
        }


        //************************ORDER TESTS****************************
        [TestMethod]
        public void show_alle_ordre()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var forventetResultat = new List<Order>();
            var order = new Order()
            {
                OrderId = 1,
                UserId = 1,
                ownerUser = new User() { UserId=1}
            };
            forventetResultat.Add(order);
            forventetResultat.Add(order);
            forventetResultat.Add(order);

            //Act

            var resultat = (ViewResult)controller.Ordre();
            var resultatListe = (List<Order>)resultat.Model;

            //Assert

            Assert.AreEqual(resultat.ViewName, "");

            for (var i = 0; i < resultatListe.Count; i++)
            {
                Assert.AreEqual(forventetResultat[i].OrderId, resultatListe[i].OrderId);
                Assert.AreEqual(forventetResultat[i].UserId, resultatListe[i].UserId);
                Assert.AreEqual(forventetResultat[i].ownerUser.UserId, resultatListe[i].ownerUser.UserId);
            }
        }

        /* 
         [TestMethod]
         public void remove_order_ok()
         {
             //Arrange
             var stub = new StoreManagerRepositoryStub();
             var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

             var forventetResultat = new List<Order>();
             var order = new Order()
             {
                 OrderId = 1,
                 UserId = 1,
                 ownerUser = new User() { UserId = 1 }
             };

             //Act

             var resultat = (ViewResult)controller.Ordre();
             var resultatListe = (List<Order>)resultat.Model;

             //Assert

             Assert.AreEqual(resultat.ViewName, "");

             for (var i = 0; i < resultatListe.Count; i++)
             {
                 Assert.AreEqual(forventetResultat[i].OrderId, resultatListe[i].OrderId);
                 Assert.AreEqual(forventetResultat[i].UserId, resultatListe[i].UserId);
                 Assert.AreEqual(forventetResultat[i].ownerUser.UserId, resultatListe[i].ownerUser.UserId);
             }
         }
         
        [TestMethod]
         public void remove_order_not_found()
         {
             //Arrange
             var stub = new StoreManagerRepositoryStub();
             var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

             var forventetResultat = new List<Order>();
             var order = new Order()
             {
                 OrderId = 1,
                 UserId = 1,
                 ownerUser = new User() { UserId = 1 }
             };

             //Act

             var resultat = (ViewResult)controller.SlettOrder(0);
             var resultatListe = (List<Order>)resultat.Model;

             //Assert

             //Assert.AreEqual(resultat.ViewName, "");

         }*/
    }
}
