using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Model;

namespace DAL
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
                new User{FirstName = "Tore" ,LastName = "Tang", Address="Tanggata 23", ZipCode=2222,UserLogin = new UserLogin{UserName = "Toretang@gmail.com", Password = genHash("1")}},
                //userid 2
                new User{FirstName = "Martin" ,LastName = "Hagen", UserLogin = new UserLogin{UserName = "Martinhagen@gmail.com", Password = genHash("BolleMann")}},
                //userid 3
                new User{FirstName = "Sondre" ,LastName = "Boge", UserLogin = new UserLogin{UserName = "Sondreboge@gmail.com", Password = genHash("WhatWhat")}},
                //userid 4
                new User{FirstName ="Store", LastName="Admin", UserLogin = new UserLogin{UserName = "admin@gmail.com", Password = genHash("123")},Admin=true}
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
            context.SaveChanges();

            foreach (var order in orders)
            {
                context.Orders.Add(order);
                Debug.WriteLine("Order added");
            }
            context.SaveChanges();
            var genres = new List<Genre>
            {
                new Genre{Name="Data", ImageUrl = "Data", Description="Ting for data"},

                new Genre{Name="Musikk", ImageUrl = "Musikk",Description="Ting for musikk"}

            };

            foreach (var genre in genres)
            {
                context.Genres.Add(genre);
            }
            context.SaveChanges();

            var salesItems = new List<SalesItem>
            {
                //id 1
                new SalesItem{Name ="Keyboard - Red", ImageUrl ="Keyboard - Red", Price=200, Description="Et rødt og stilig keyboard!", InStock=10, Genre = genres.Single(g=>g.Name=="Data")},
                //id 2
                new SalesItem{Name ="Keyboard - Black", ImageUrl ="Keyboard - Black",  Price=250, Description="Sykt stilig svart keyboard!", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 3
                new SalesItem{Name ="Commandore 64", ImageUrl ="Commandore 64",  Price=200, Description="Sykt gammelt, men stilig keyboard!", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 4
                new SalesItem{Name ="Keyboard - Android", ImageUrl ="Keyboard - Android",  Price=300, Description="Android, what more to say?", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 5
                new SalesItem{Name ="Keyboard - Bended", ImageUrl ="Keyboard - Bended",  Price=150, Description="Sykt billig og stilig og ikke minst bøyelig keyboard, som bare varer og varer og varer og varer enda lenger. Den snille prisen gjenspeiler hvor fantastisk denne bedriften er, og vi håper å kunne fortsette å holde det slik.", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 6
                new SalesItem{Name ="Keyboard - Drawing", ImageUrl ="Keyboard - Drawing",  Price=200, Description="Et fint lite tastatur konsept, obs bare tegning er inkludert.", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 7
                new SalesItem{Name ="Keyboard - Enthusiast", ImageUrl ="Keyboard - Enthusiast",  Price=700, Description="For en som har masse penger og liker at ting skal se dyrt ut. En slags futuristisk look.", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 8
                new SalesItem{Name ="Keyboard - Futuristic", ImageUrl ="Keyboard - Futuristic", Price=200, Description="Det egentlige futuristiske tastaturet på Tast-En-Tur. Vårt kjære lille barn som vi alltid vil elske.", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 9
                new SalesItem{Name ="Keyboard - Half Wierd", ImageUrl ="Keyboard - Half Wierd", Price=300, Description="Et halvveis normalt keyboard, som passer mødre som vil føle seg litt moderne.", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 10
                new SalesItem{Name ="Keyboard - New Rainbow", ImageUrl ="Keyboard - New Rainbow", Price=200, Description="Moderne og fargerikt!", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 11
                new SalesItem{Name ="Keyboard - No Case", ImageUrl ="Keyboard - No Case", Price=1000, Description="Om du ikke liker plastikk, men elsker elektronikk.", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 12
                new SalesItem{Name ="Keyboard - Rainbow", ImageUrl ="Keyboard - Rainbow", Price=2000, Description="Farger som minner om hippie-tiden!", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 13
                new SalesItem{Name ="Keyboard - Right Handed", ImageUrl ="Keyboard - Right Handed", Price=2000, Description="Kjøp en venstre-utgave og bli dobbelt så effektiv!", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 14
                new SalesItem{Name ="Keyboard - Virtual", ImageUrl ="Keyboard - Virtual", Price=1000, Description="Nå trenger du ikke klunkete tastatur, hvorfor ikke slå ihjel fingrene på bordet ditt?", InStock=10,Genre = genres.Single(g=>g.Name=="Data")},
                //id 15
                new SalesItem{Name ="Musical Keyboard - Retro", ImageUrl ="Musical Keyboard - Retro", Price=1000, Description="Musikk, for de som ikke føler seg like unge lenger, og savner de gode gamle dager.", InStock=10,Genre = genres.Single(g=>g.Name=="Musikk")},
                //id 16
                new SalesItem{Name ="Musical Keyboard with iPad", ImageUrl ="Musical Keyboard with iPad", Price=200, Description="Om du mangler både keyboard og overprisa en iPad.", InStock=10,Genre = genres.Single(g=>g.Name=="Musikk")},
                //id 17
                new SalesItem{Name ="Musical Keyboard", ImageUrl ="Musical Keyboard", Price=700, Description="Standard utgaven for musikk keyboardene.", InStock=10,Genre = genres.Single(g=>g.Name=="Musikk")},
                
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