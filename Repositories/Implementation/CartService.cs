using eBookStore.Models;
using eBookStore.Repositories.Abstract;

namespace eBookStore.Repositories.Implementation;

public class CartService(AppDbContex context) : ICartService
{
    bool ICartService.AddToCart(Cart cart)
    {
        try
        {
            context.Add(cart);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

    bool ICartService.ClearCart(int UserID)
    {
        throw new NotImplementedException();
    }

    IEnumerable<Cart> ICartService.GetCartItemByUser(int id)
    {
        throw new NotImplementedException();
    }
}
