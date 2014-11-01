using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using WebAppH2014.Controllers;
using BLL;
using DAL;
using Model;
using System.Web;

namespace Enhetstest
{
    [TestClass]
    public class StoreManagerControllerTest
    {
        
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
    }
}
