using eBookStore.Models;
using eBookStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace eBookStore.Controllers;

public class AdminController(IUserService service, IBookService bookService) : Controller
{
    public IActionResult Index()
    {
        var userRole = HttpContext.Session.GetString("UserRole");

        if (userRole != "Admin")
        {
            return RedirectToAction("Login", "Account"); // Redirect non-admin users to login page
        }

        return View();
    }

    [HttpGet]
    public IActionResult Profile()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        var userEmail = HttpContext.Session.GetString("UserEmail");

        if (userRole != "Admin")
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


    [HttpGet]
    public IActionResult User()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        var userEmail = HttpContext.Session.GetString("UserEmail");

        if (userRole != "Admin")
        {
            return RedirectToAction("Login", "Account"); // Redirect non-admin users to login page
        }

        var user = service.GetAllUsers();
        var filteredUser = user.Where(u => u.Email != userEmail).ToList();

        return View(filteredUser);

    }

    [HttpPost]
    public IActionResult DeleteUser(string email)
    {
        if (email == null)
        {
            return View("Index");
        }

        var deleteUser = service.DeleteUser(email);

        if (deleteUser)
        {
            TempData["SuccessMessage"] = "User delted successfully!";
            return this.User();
        }

        ViewBag.ErrorMessage = "Error occured please try again !";
        return View("User");
    }

    public IActionResult AddUser()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddUser(User user)
    {
        if (ModelState.IsValid)
        {
            // Check if the email already exists in the database
            var emailExists = service.GetUserById(user.Email);
            if (emailExists != null)
            {
                if (emailExists.Email == user.Email)
                {
                    ViewBag.ErrorMessage = "Email already exists. Please use a different email.";
                    return View("User"); // Return the same view with the error message
                }
            }

            // Set default password
            user.Password = "123";

            // Save user to database
            var results = service.AddUser(user);

            if (results)
            {
                // Set success message
                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction("User"); // Redirect to a user listing page
            }

            ViewBag.ErrorMessage = "Something went wrong! Try again.";
            return View("User");
        }
        else
        {
            // If ModelState is invalid
            ViewBag.ErrorMessage = "Invalid data. Please check the fields and try again.";
            return View("User"); // Return the same view with validation errors
        }
    }
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
    public IActionResult ViewBook(int id)
    {
        var book = bookService.GetBookById(id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    [HttpPost]
    public IActionResult UpdateBook(Book book)
    {
        if (book == null)
        {
            return View("Index");
        }

        var update = bookService.UpdateBook(book);

        if (update)
        {
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return View("ViewBook", book);
        }

        ViewBag.ErrorMessage = "Error occured please try again !";
        return View("ViewBook");
    }


    [HttpPost]
    public IActionResult DeleteBook(int id)
    {
        if (id == null)
        {
            return View("Index");
        }

        var deleteBook = bookService.DeleteBook(id);

        if (deleteBook)
        {
            TempData["SuccessMessage"] = "Book delted successfully!";
            return View("Books");
        }

        ViewBag.ErrorMessage = "Error occured please try again !";
        return View("Books");
    }

    [HttpGet]
    public IActionResult AddBook()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddBook(Book book)
    {
        if (ModelState.IsValid)
        {

            // Set default image
            book.ImageURL = "";

            // Save user to database
            var results = bookService.AddBook(book);

            if (results)
            {
                // Set success message
                TempData["SuccessMessage"] = "Book added successfully!";
                return RedirectToAction("Books"); // Redirect to a user listing page
            }

            ViewBag.ErrorMessage = "Something went wrong! Try again.";
            return View("Books");
        }
        else
        {
            // If ModelState is invalid
            ViewBag.ErrorMessage = "Invalid data. Please check the fields and try again.";
            return View("books"); // Return the same view with validation errors
        }
    }
}

