using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebAppH2014.Models
{
    public class StoreInitializer : DropCreateDatabaseIfModelChanges<StoreContext>
    {

        protected override void Seed(StoreContext context)
        {
            Debug.WriteLine("InInit");
            //ANON OBJECTS FOR DATABASE-SEEDING
            var users = new List<User>
            {
                //userId 1
                new User{FirstName = "Tore" ,LastName = "Tang", UserLogin = new UserLogin{UserName = "Toretang", Password = genHash("GammelMann")}},
                //userid 2
                new User{FirstName = "Martin" ,LastName = "Hagen", UserLogin = new UserLogin{UserName = "Martinhagen", Password = genHash("BolleMann")}},
                //userid 3
                new User{FirstName = "Sondre" ,LastName = "Boge", UserLogin = new UserLogin{UserName = "Sondreboge", Password = genHash("WhatWhat")}},
            };

            Debug.WriteLine("Users Created");
            var orders = new List<Order>();
            foreach (var user in users)
            {
                //TODO add orders to user
                context.Users.Add(user);

                //OrderId 1-3 generated
                orders.Add(new Order { ownerUser = user });
                Debug.WriteLine("User added");
            }

            foreach (var order in orders)
            {
                context.Orders.Add(order);
                Debug.WriteLine("Order added");
            }

            var genres = new List<Genre>
            {
                new Genre{Name="Data", Description="Stuff for data"},

                new Genre{Name="Music", Description="Stuff for music"}

            };

            foreach (var genre in genres)
            {
                context.Genres.Add(genre);
            }
            context.SaveChanges();

            var salesItems = new List<SalesItem>
            {
                //id 1
                new SalesItem{Name ="Keyboard - Red", Price=200, Description="Sykt rødt og stilig keyboard!", InStock=10, Genre = genres.Single(g=>g.Name=="Data")},
                //id 2
                new SalesItem{Name ="Keyboard - Black", Price=250, Description="Sykt stilig svart keyboard!", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 3
                new SalesItem{Name ="Commandore 64", Price=200, Description="Sykt gammelt, men stilig keyboard!", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 4
                new SalesItem{Name ="Keyboard - Android", Price=300, Description="Anroid, what more to say?", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 5
                new SalesItem{Name ="Keyboard - Bended", Price=150, Description="Sykt billig og stilig og ikke minst bøyelig keyboard, som bare varer og varer og varer og varer enda lenger. Den snille prisen gjenspeiler hvor awesome denne bedriften er og vi håper å kunne fortsette å holde det slik.", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 6
                new SalesItem{Name ="Keyboard - Drawing", Price=200, Description="Et fint lite tastatur konsept, obs bare tegning er inkl.", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 7
                new SalesItem{Name ="Keyboard - Enthusiast", Price=700, Description="For en som har masse penger og vil at ting skal se dyrt ut. En slags futuristisk look.", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 8
                new SalesItem{Name ="Keyboard - Futuristic", Price=200, Description="Det egentlige futuristiske tastaturet på Tast-En-Tur. Vårt kjære lille barn som vi alltid vil elske.", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 9
                new SalesItem{Name ="Keyboard - Half Wierd", Price=300, Description="Et halvveis normalt keyboard, som passer mødre som vil føle seg moderne.", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 10
                new SalesItem{Name ="Keyboard - Modern Rainbow", Price=200, Description="Moderne og fargerikt!", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 11
                new SalesItem{Name ="Keyboard - No Case", Price=1000, Description="Om du ikke liker plastikk, men elsker elektronikk.", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 12
                new SalesItem{Name ="Keyboard - Rainbow", Price=2000, Description="Nå slipper du å ta drugs!", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 13
                new SalesItem{Name ="Keyboard - Right Handed", Price=2000, Description="Om venstre hånda di er opptatt...", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 14
                new SalesItem{Name ="Keyboard - Virtual", Price=1000, Description="Nå trenger du ikke klunkete tastatur, hvorfor ikke slå ihjel fingrene på bordet ditt?", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 15
                new SalesItem{Name ="Musical Keyboard - Retro", Price=1000, Description="Musikk, for de som ikke føler seg like unge lenger, og savner de gode gamle dager.", InStock=10,Genre = genres.Single(g=>g.Name=="Music")},
                //id 16
                new SalesItem{Name ="Musical Keyboard with iPad", Price=200, Description="Om du mangler både keyboard og overprisa en iPad.", InStock=10,Genre = genres.Single(g=>g.Name=="Music")},
                //id 17
                new SalesItem{Name ="Musical Keyboard", Price=700, Description="Standard utgaven for musikk keyboardene.", InStock=10,Genre = genres.Single(g=>g.Name=="Music")},
                
            };

            foreach (var vare in salesItems)
            {
                context.SalesItems.Add(vare);
            }

            context.SaveChanges();
            //This part currently gives an error

            var orderItems = new List<OrderSalesItem>
            {
                new OrderSalesItem{OrderId=1, SalesItemId=1, Amount=2 },
                new OrderSalesItem{OrderId=1, SalesItemId=2, Amount=1},
                new OrderSalesItem{OrderId=3, SalesItemId=1, Amount=4 },
                new OrderSalesItem{OrderId=2, SalesItemId=2, Amount=15 },
            };

            foreach (var orderItem in orderItems)
            {
                context.SalesItemInOrder.Add(orderItem);
            }




            context.SaveChanges();
        }
        private static byte[] genHash(string inPassword)
        {
            byte[] inData, outData;
            var algorithm = System.Security.Cryptography.SHA256.Create();
            inData = System.Text.Encoding.ASCII.GetBytes(inPassword);
            outData = algorithm.ComputeHash(inData);
            return outData;
        }

    }
}