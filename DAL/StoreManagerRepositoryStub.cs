﻿using Model;
using System.Collections.Generic;

namespace DAL
{
    public class StoreManagerRepositoryStub : DAL.IStoreManagerRepository
    {
        public bool addSalesItem(SalesItem item)
        {
            if (item.Name == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool removeSalesItem(SalesItem item)
        {
            if (item.Name == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool editSalesItem(SalesItem item)
        {
            if (item.Name == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<SalesItem> getSalesItemsWithGenre()
        {
            var salesItems = new List<SalesItem>();
            var item = new SalesItem()
            {
                SalesItemId = 1,
                Price = new decimal(100.0),
                Name = "Keyboard",
                Description = "Fint keyboard",
                InStock = 10,
                GenreId = 1,
                ImageUrl = "bilde"
            };
            salesItems.Add(item);
            salesItems.Add(item);
            salesItems.Add(item);
            return salesItems;
        }


        public List<User> getUsers()
        {
            var users = new List<User>();
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
            users.Add(user);
            users.Add(user);
            users.Add(user);
            return users;
        }


        public List<Order> getOrders()
        {
            var orders = new List<Order>();
            var order = new Order()
            {
                OrderId = 1,
                UserId = 1,
                ownerUser = new User()
            };
            orders.Add(order);
            orders.Add(order);
            orders.Add(order);
            return orders;
        }

        public void editUser(int userId, User user)
        {
            return;
        }

        public void removeUser(User user)
        {
            return;
        }
    }
}
