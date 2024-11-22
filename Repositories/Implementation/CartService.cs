using eBookStore.Models;
using eBookStore.Repositories.Abstract;

namespace eBookStore.Repositories.Implementation;

public class CartService(AppDbContext context) : ICartService
{
    public bool DeleteCartById(int BookID, int UserID)
    {
        try
        {
            var data = this.GetCartById(BookID, UserID);
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

    public Cart GetCartById(int BookId, int Userid)
    {
        var cart = context.Cart.FirstOrDefault(x => x.UserID == Userid && x.BookID == BookId);
        return cart;
    }

    public bool UpdateCart(Cart cart)
    {
        try
        {
            context.Cart.Update(cart);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

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
        return context.Cart.Where(x => x.UserID == id);
    }
}
