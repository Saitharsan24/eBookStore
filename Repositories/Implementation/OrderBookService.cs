using eBookStore.Models;
using eBookStore.Repositories.Abstract;

namespace eBookStore.Repositories.Implementation;

public class OrderBookService(AppDbContext context) : IOrderBookService
{
    bool IOrderBookService.AddOrderBook(IEnumerable<OrderBook> orderBooks)
    {
        try
        {
            context.OrderBook.AddRange(orderBooks);
            context.SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            // Log exception (optional)
            Console.WriteLine($"Error occurred while saving OrderBooks: {ex.Message}");
            return false; // Return false if an error occurs
        }
    }

    IEnumerable<OrderBook> IOrderBookService.GetOrderBooks(int OrderID)
    {
        var orderBooks = context.OrderBook.Where(x => x.OrderID == OrderID);
        return orderBooks;
    }

    IEnumerable<OrderBook> IOrderBookService.GetAllOrderBooks()
    {
        var orderBooks = context.OrderBook.ToList();
        return orderBooks;
    }
}

