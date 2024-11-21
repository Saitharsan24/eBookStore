using eBookStore.Models;
using eBookStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace eBookStore.Controllers;

public class CustomerController(IOrderBookService orderBookService, IOrderService orderService, IBookService bookService, IUserService service, IFeedBackService feedBackService, ICartService cartService) : Controller
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

    [HttpPost]
    public IActionResult AddToCart(int BookID, int quantity)
    {
        var userID = HttpContext.Session.GetString("UserID");
        var id = int.TryParse(userID, out int intuserId);
        var book = bookService.GetBookById(BookID);
        var cartItem = cartService.GetCartById(BookID, intuserId);

        if (cartItem != null)
        {
            cartItem.Quantity = quantity;
            var results = cartService.UpdateCart(cartItem);
            if (results)
            {
                TempData["BookSuccessMessage"] = "Item added to cart successfully!";
                return RedirectToAction("ViewBook", new { BookID });
            }

            TempData["BookErrorMessage"] = "Error occurred ! Try again.";
            return RedirectToAction("ViewBook", new { BookID });
        }

        Cart cart = new Cart();
        cart.Quantity = quantity;
        cart.Author = book.Author;
        cart.Title = book.Title;
        cart.Price = book.Price * quantity;
        cart.UserID = intuserId;
        cart.BookID = BookID;
        cart.ImageURL = book.ImageURL;

        var result = cartService.AddToCart(cart);

        if (result)
        {
            TempData["BookSuccessMessage"] = "Item added to cart successfully!";
            return RedirectToAction("ViewBook", new { BookID });
        }

        TempData["BookErrorMessage"] = "Error occurred ! Try again.";
        return RedirectToAction("ViewBook", new { BookID });
    }

    public IActionResult Cart()
    {
        int.TryParse(HttpContext.Session.GetString("UserID"), out int intuserId);
        var cartTtems = cartService.GetCartItemByUser(intuserId);

        return View(cartTtems);
    }

    public IActionResult Order()
    {
        int.TryParse(HttpContext.Session.GetString("UserID"), out int intuserId);
        var orders = orderService.GetOrdersByUserId(intuserId);

        return View(orders);
    }

    public IActionResult ViewOrder(int OrderID)
    {
        var order = orderService.GetOrderById(OrderID);
        var orderBooks = orderBookService.GetOrderBooks(OrderID);

        OrderViewModel orderView = new OrderViewModel();

        orderView.OrderDate = order.OrderDate;
        orderView.OrderId = order.OrderID;
        orderView.TotalPrice = order.TotalPrice;
        orderView.status = order.status;
        orderView.OrderedBooks = orderBooks;

        List<Book> books = new List<Book>();

        foreach (var book in orderBooks)
        {
            var bookObject = bookService.GetBookById(book.BookID);
            books.Add(bookObject);
        }

        var booksDictionary = books.ToDictionary(book => book.BookID);
        orderView.Books = booksDictionary;

        return View(orderView);
    }

    public IActionResult CancelOrder(int OrderID)
    {
        var order = orderService.GetOrderById(OrderID);
        order.status = "Cancelled";

        var resutls = orderService.UpdateOrder(order);

        if (resutls)
        {
            TempData["BookSuccessMessage"] = "Order cancelled successfully!";
            return RedirectToAction("ViewOrder", new { OrderID });
        }

        TempData["BookErrorMessage"] = "Error occurred ! Try again.";
        return RedirectToAction("ViewOrder", new { OrderID });

    }
}
