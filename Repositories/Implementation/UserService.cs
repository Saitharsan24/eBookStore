using eBookStore.Models;
using eBookStore.Repositories.Abstract;

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

    public bool DeleteUser(int id)
    {
        try
        {
            var data = this.GetUserById(id);
            if (data != null)
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

    public User GetUserById(int id)
    {
        return context.User.Find(id);
    }

    public bool UpdateUser(User user)
    {
        try
        {
            context.User.Update(user);
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
