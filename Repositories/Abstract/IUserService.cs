using eBookStore.Models;

namespace eBookStore.Repositories.Abstract;

public interface IUserService
{
    bool AddUser(User user);
    bool UpdateUser(User user);
    bool DeleteUser(string email);
    User GetUserById(string email);
    User GetUserById(int ID);
    IEnumerable<User> GetAllUsers();
}
