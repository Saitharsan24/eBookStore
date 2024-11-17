using eBookStore.Models;
using eBookStore.Repositories.Abstract;

namespace eBookStore.Repositories.Implementation;

public class OrderService(AppDbContex context) : IOrderService
{
    public bool AddOrder(Order order)
    {
        try
        {
            context.Add(order);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

    public bool DeleteOrder(int id)
    {
        try
        {
            var data = this.GetOrderById(id);
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

    public IEnumerable<Order> GetAllOrders()
    {
        return context.Order.ToList();
    }

    public Order GetOrderById(int id)
    {
        return context.Order.Find(id);
    }

    public IEnumerable<Order> GetOrdersByUserId(int userID)
    {
        return context.Order.Where(x => x.UserID == userID);
    }

    public bool UpdateOrder(Order order)
    {
        try
        {
            context.Order.Update(order);
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
