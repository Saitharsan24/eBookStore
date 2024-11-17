using eBookStore.Models;
using eBookStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace eBookStore.Repositories.Implementation;

public class UserService(AppDbContex context) : IUserService
{
    public bool AddUser(User user)
    {
        try
        {
            context.Add(user);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

    [HttpPost]
    public bool DeleteUser(string email)
    {
        try
        {
            var data = this.GetUserById(email);
            if (data == null)
            {
                return false;
            }

            context.Remove(data);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

    public IEnumerable<User> GetAllUsers()
    {
        return context.User.ToList();
    }

    public User GetUserById(string email)
    {
        return context.User.FirstOrDefault(x => x.Email == email);
    }

    public bool UpdateUser(User user)
    {
        try
        {
            var userObject = GetUserById(user.Email);
            userObject.Name = user.Name;
            userObject.Password = user.Password;

            context.User.Update(userObject);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }
}
