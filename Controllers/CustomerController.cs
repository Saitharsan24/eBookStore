using eBookStore.Models;
using eBookStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace eBookStore.Controllers;

public class CustomerController(IBookService bookService, IUserService service, IFeedBackService feedBackService) : Controller
{
    public IActionResult Books(string searchString)
    {
        // Retrieve books from the database
        var books = bookService.GetAllBooks();

        // Filter books if search string is provided
        if (!string.IsNullOrEmpty(searchString))
        {
            books = books.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString));
        }

        return View(books.ToList());
    }

    [HttpGet]
    public IActionResult Profile()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        var userEmail = HttpContext.Session.GetString("UserEmail");

        if (userRole != "Customer")
        {
            return RedirectToAction("Login", "Account"); // Redirect non-admin users to login page
        }

        var user = service.GetUserById(userEmail);

        return View(user);

    }

    [HttpPost]
    public IActionResult UpdateProfile(User user)
    {
        if (user == null)
        {
            return View("Index");
        }

        var update = service.UpdateUser(user);

        if (update)
        {
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return View("Profile", user);
        }

        ViewBag.ErrorMessage = "Error occured please try again !";
        return View("Profile");
    }

    public IActionResult ViewBook(int id)
    {
        var results = bookService.GetBookById(id);
        var feedBack = feedBackService.GetFeedBacksByBookId(id);

        BookViewModel bookViewModel = new BookViewModel();

        bookViewModel.Title = results.Title;
        bookViewModel.Price = results.Price;
        bookViewModel.Stock = results.Stock;
        bookViewModel.BookID = results.BookID;
        bookViewModel.Author = results.Author;
        bookViewModel.Category = results.Category;
        bookViewModel.ImageURL = results.ImageURL;
        bookViewModel.FeedBacks = feedBack;

        return View(bookViewModel);
    }

    [HttpPost]
    public IActionResult AddFeedback(int id, string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
        {
            TempData["ErrorMessage"] = "Comment cannot be empty.";
            return RedirectToAction("ViewBook", new { id });
        }

        var userName = HttpContext.Session.GetString("UserName");

        var feedback = new FeedBack
        {
            UserName = userName,
            BookID = id,
            Comment = comment
        };

        var results = feedBackService.AddFeedBack(feedback);

        if (results)
        {
            TempData["SuccessMessage"] = "Feedback submitted successfully!";
            return RedirectToAction("ViewBook", new { id });
        }

        TempData["ErrorMessage"] = "Error occurred ! Try again.";
        return RedirectToAction("ViewBook", new { id });

    }
}
