using eBookStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace eBookStore.Controllers
{
    public class HomeController(IBookService service) : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Books(string searchString)
        {
            // Retrieve books from the database
            var books = service.GetAllBooks();

            // Filter books if search string is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString));
            }

            return View(books.ToList());
        }


    }
}
