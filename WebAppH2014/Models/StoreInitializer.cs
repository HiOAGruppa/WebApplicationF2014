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
                new User{UserName = "Tore Tang", UserLogin = new UserLogin{UserName = "Toretang", Password = genHash("GammelMann")}},
                //userid 2
                new User{UserName = "Martin Hagen", UserLogin = new UserLogin{UserName = "Martinhagen", Password = genHash("BolleMann")}},
                //userid 3
                new User{UserName = "Sondre Boge", UserLogin = new UserLogin{UserName = "Sondreboge", Password = genHash("WhatWhat")}},
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

            var salesItems = new List<SalesItem>
            {
                //id 1
                new SalesItem{Name ="Keyboard-Red", Price=200, Description="Sykt stilig keyboard", InStock=10},
                //id 2
                new SalesItem{Name ="Keyboard-Black", Price=250, Description="Sykt stilig keyboard", InStock=10},
                //id 1
                new SalesItem{Name ="Keyboard-Red", Price=200, Description="Sykt stilig keyboard", InStock=10},
                //id 2
                new SalesItem{Name ="Keyboard-Black", Price=300, Description="Sykt stilig keyboard", InStock=10},
                                //id 1
                new SalesItem{Name ="Keyboard-Red", Price=150, Description="Sykt billig og stilig keyboard", InStock=10},
                //id 2
                new SalesItem{Name ="Keyboard-Black", Price=200, Description="Sykt stilig keyboard", InStock=10},
                                //id 1
                new SalesItem{Name ="Keyboard-Red", Price=700, Description="Sykt stilig keyboard", InStock=10},
                //id 2
                new SalesItem{Name ="Keyboard-Black", Price=200, Description="Sykt stilig keyboard", InStock=10},
                                //id 1
                new SalesItem{Name ="Keyboard-Red", Price=300, Description="Sykt stilig keyboard", InStock=10},
                //id 2
                new SalesItem{Name ="Keyboard-Black", Price=200, Description="Sykt stilig keyboard", InStock=10},
                                //id 1
                new SalesItem{Name ="Keyboard-Red", Price=1000, Description="Sykt stilig keyboard", InStock=10},
                //id 2
                new SalesItem{Name ="Keyboard-Black", Price=2000, Description="Sykt stilig keyboard", InStock=10},
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