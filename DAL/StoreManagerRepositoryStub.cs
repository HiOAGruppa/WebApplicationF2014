using Model;
using System.Collections.Generic;

namespace DAL
{
    public class StoreManagerRepositoryStub : DAL.IStoreManagerRepository
    {
        public bool addSalesItem(SalesItem item)
        {
            if (item.Name == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool removeSalesItem(SalesItem item)
        {
            if (item.Name == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool editSalesItem(SalesItem item)
        {
            if (item.SalesItemId == 0)
            {
                return false;
            }
            else
            {
                return true;
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

        public SalesItem findSalesItem(int id)
        {
            if (id == 0)
            {
                var item = new SalesItem();
                item.SalesItemId = 0;
                return item;
            }
            else
            {
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
                return item;
            }
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

        public User getUser(int id)
        {
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
            return user;
        }
        public List<Order> getOrders()
        {
            var orders = new List<Order>();
            var order = new Order()
            {
                OrderId = 1,
                UserId = 1,
                ownerUser = new User() { UserId = 1 }
            };
            orders.Add(order);
            orders.Add(order);
            orders.Add(order);
            return orders;
        }

        public bool editUser(int userId, User user)
        {
            if (user.UserId == 0)
                return false;
            else
                return true;
        }

        public bool removeUser(User user)
        {
            if (user.UserId == 0)
                return false;
            else
                return true;
        }

        public bool removeOrder(int id)
        {
            if (id == 0)
                return false;
            else
                return true;
        }
    }
}
