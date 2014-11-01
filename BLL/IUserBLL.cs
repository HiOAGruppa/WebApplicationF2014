using System;
namespace BLL
{
    public interface IUserBLL
    {
        void addUser(Model.User user, Model.UserLogin login);
        bool editUser(int userId, Model.User user);
        Model.UserLogin findUserLoginByPassword(byte[] passwordhash, string username);
        Model.User getUser(int userId);
        System.Collections.Generic.List<Model.User> getUsers();
        bool removeUser(Model.User user);
        bool usernameExists(string username);
        bool verifyUser(Model.UserModifyUser inUser);
    }
}
