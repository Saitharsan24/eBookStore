using eBookStore.Models;

namespace eBookStore.Repositories.Abstract;

public interface IOrderService
{
    int AddOrder(Order order);
    bool UpdateOrder(Order order);
    bool DeleteOrder(int id);
    Order GetOrderById(int id);
    IEnumerable<Order> GetAllOrders();
    IEnumerable<Order> GetOrdersByUserId(int id);
}
