using eBookStore.Models;
using eBookStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;

namespace eBookStore.Controllers;

public class AdminController(IOrderBookService orderBookService, IOrderService orderService, IUserService service, IBookService bookService) : Controller
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
            return RedirectToAction("User");
        }

        ViewBag.ErrorMessage = "Error occured please try again !";
        return RedirectToAction("User");
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
                    return RedirectToAction("User"); // Return the same view with the error message
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
            return RedirectToAction("User");
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
            return RedirectToAction("Index");
        }

        var deleteBook = bookService.DeleteBook(id);

        if (deleteBook)
        {
            TempData["SuccessMessage"] = "Book deleted successfully!";
            return RedirectToAction("Books");
        }

        ViewBag.ErrorMessage = "Error occured please try again !";
        return RedirectToAction("Books");
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
            return RedirectToAction("Books");
        }
        else
        {
            // If ModelState is invalid
            ViewBag.ErrorMessage = "Invalid data. Please check the fields and try again.";
            return RedirectToAction("books"); // Return the same view with validation errors
        }
    }

    public IActionResult Orders()
    {
        var orders = orderService.GetAllOrders();

        return View(orders);
    }

    public IActionResult ViewOrder(int OrderID)
    {
        var orders = orderService.GetOrderById(OrderID);
        var user = service.GetUserById(orders.OrderID);
        var orderBooks = orderBookService.GetOrderBooks(OrderID);

        OrderViewModel orderViewModel = new OrderViewModel();

        orderViewModel.user = user;
        orderViewModel.OrderDate = orders.OrderDate;
        orderViewModel.OrderId = orders.OrderID;
        orderViewModel.TotalPrice = orders.TotalPrice;
        orderViewModel.status = orders.status;
        orderViewModel.OrderedBooks = orderBooks;

        List<Book> books = new List<Book>();

        foreach (var book in orderBooks)
        {
            var bookObject = bookService.GetBookById(book.BookID);
            books.Add(bookObject);
        }

        var booksDictionary = books.ToDictionary(book => book.BookID);
        orderViewModel.Books = booksDictionary;

        return View(orderViewModel);
    }

    public IActionResult ShipOrder(int OrderID)
    {
        var order = orderService.GetOrderById(OrderID);
        order.status = "Shipped";

        var resutls = orderService.UpdateOrder(order);

        if (resutls)
        {
            TempData["CancelSuccessMessage"] = "Order Shipped successfully!";
            return RedirectToAction("ViewOrder", new { OrderID });
        }

        TempData["CancelErrorMessage"] = "Error occurred ! Try again.";
        return RedirectToAction("ViewOrder", new { OrderID });

    }

    public IActionResult CompleteOrder(int OrderID)
    {
        var order = orderService.GetOrderById(OrderID);
        order.status = "Completed";

        var resutls = orderService.UpdateOrder(order);

        if (resutls)
        {
            TempData["CancelSuccessMessage"] = "Order Completed successfully!";
            return RedirectToAction("ViewOrder", new { OrderID });
        }

        TempData["CancelErrorMessage"] = "Error occurred ! Try again.";
        return RedirectToAction("ViewOrder", new { OrderID });

    }

    public IActionResult CancelOrder(int OrderID)
    {
        var order = orderService.GetOrderById(OrderID);
        order.status = "Cancelled";

        var resutls = orderService.UpdateOrder(order);

        if (resutls)
        {
            TempData["CancelSuccessMessage"] = "Order cancelled successfully!";
            return RedirectToAction("ViewOrder", new { OrderID });
        }

        TempData["CancelErrorMessage"] = "Error occurred ! Try again.";
        return RedirectToAction("ViewOrder", new { OrderID });

    }

    public IActionResult Reports()
    {
        return View();
    }

    public IActionResult GenerateOrdersReport(DateTime startDate, DateTime endDate, string status)
    {
        var orders = orderService.GetAllOrders();

        var orderDict = new List<OrderViewModel>();

        foreach (var order in orders)
        {
            var user = service.GetUserById(order.UserID);
            var orderBooks = orderBookService.GetOrderBooks(order.OrderID);

            OrderViewModel orderViewModel = new OrderViewModel();

            orderViewModel.user = user;
            orderViewModel.OrderDate = order.OrderDate;
            orderViewModel.OrderId = order.OrderID;
            orderViewModel.TotalPrice = order.TotalPrice;
            orderViewModel.status = order.status;
            orderViewModel.OrderedBooks = orderBooks;

            List<Book> books = new List<Book>();

            foreach (var book in orderBooks)
            {
                var bookObject = bookService.GetBookById(book.BookID);
                books.Add(bookObject);
            }

            var booksDictionary = books.ToDictionary(book => book.BookID);
            orderViewModel.Books = booksDictionary;

            orderDict.Add(orderViewModel);
        }

        var filteredOrders = orderDict.Where(order => order.OrderDate >= startDate && order.OrderDate <= endDate
                                    && (status == null || order.status == status)).ToList();


        var reportData = GenerateOrdersCsv(filteredOrders);
        var fileName = $"OrdersReport_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";

        return File(Encoding.UTF8.GetBytes(reportData), "text/csv", fileName);
    }

    public IActionResult GenerateUsersReport()
    {
        var users = service.GetAllUsers();
        var csvData = GenerateUsersCsv(users);

        var fileName = $"UsersReport_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
        return File(Encoding.UTF8.GetBytes(csvData), "text/csv", fileName);
    }

    // Helper to generate CSV for Orders
    private string GenerateOrdersCsv(IEnumerable<OrderViewModel> orders)
    {
        var csv = new StringBuilder();
        csv.AppendLine("OrderId,CustomerName,OrderDate,TotalPrice,Status");

        foreach (var order in orders)
        {
            csv.AppendLine($"{order.OrderId},{order.user.Name}," +
                $"{order.OrderDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}," +
                $"{order.TotalPrice},{order.status}");
        }

        return csv.ToString();
    }

    // Helper to generate CSV for users
    private string GenerateUsersCsv(IEnumerable<User> users)
    {
        var csv = new StringBuilder();
        csv.AppendLine("UserId,Name,Email,Role");

        foreach (var user in users)
        {
            csv.AppendLine($"{user.UserID},{user.Name},{user.Email}," +
                $"{user.Role}");
        }

        return csv.ToString();
    }
}

