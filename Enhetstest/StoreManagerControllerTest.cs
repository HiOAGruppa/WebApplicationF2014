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

        public void show_delete_item_view()
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
        public void delete_item_OK()
        {
            //Arrange
            var stub = new StoreManagerRepositoryStub();
            var controller = new StoreManagerController(new SalesItemBLL(stub), new UserBLL(stub), new OrderBLL(stub), new GenreBLL(stub));

            var inItem = new SalesItem();
            inItem.SalesItemId = 1;

            //Act
            var resultat = (RedirectToRouteResult)controller.DeleteConfirmed(inItem.SalesItemId);

            //Assert
            Assert.AreEqual(resultat.RouteName, "");
            Assert.AreEqual(resultat.RouteValues.Values.First(), "Index");
        }
    }
}
