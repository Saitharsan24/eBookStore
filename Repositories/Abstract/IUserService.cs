using eBookStore.Models;

namespace eBookStore.Repositories.Abstract;

public interface IUserService
{
    bool AddUser(User user);
    bool UpdateUser(User user);
    bool DeleteUser(int id);
    User GetUserById(int id);
    IEnumerable<User> GetAllUsers();
}
