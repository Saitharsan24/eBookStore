using eBookStore.Models;

namespace eBookStore.Repositories.Abstract;

public interface IBookService
{
    bool AddBook(Book book);
    bool UpdateBook(Book book);
    bool DeleteBook(int id);
    Book GetBookById(int id);
    IEnumerable<Book> GetAllBooks();

}
