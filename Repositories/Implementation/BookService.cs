using eBookStore.Models;
using eBookStore.Repositories.Abstract;

namespace eBookStore.Repositories.Implementation;

public class BookService(AppDbContex context) : IBookService
{
    public bool AddBook(Book book)
    {
        try
        {
            context.Add(book);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

    public bool DeleteBook(int id)
    {
        try
        {
            var data = this.GetBookById(id);
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

    public IEnumerable<Book> GetAllBooks()
    {
        return context.Book.ToList();
    }

    public Book GetBookById(int id)
    {
        return context.Book.Find(id);
    }

    public bool UpdateBook(Book book)
    {
        try
        {
            context.Book.Update(book);
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
