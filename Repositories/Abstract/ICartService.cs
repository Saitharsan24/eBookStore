using eBookStore.Models;

namespace eBookStore.Repositories.Abstract;

public interface ICartService
{
    bool AddToCart(Cart cart);
    bool ClearCart(int UserID);
    IEnumerable<Cart> GetCartItemByUser(int id);
}
