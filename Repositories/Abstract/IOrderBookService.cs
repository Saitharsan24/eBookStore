using eBookStore.Models;

namespace eBookStore.Repositories.Abstract;

public interface IOrderBookService
{
    bool AddOrderBook(IEnumerable<OrderBook> orderBooks);
    IEnumerable<OrderBook> GetOrderBooks(int OrderID);
}
