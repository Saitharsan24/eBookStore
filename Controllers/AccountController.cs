using eBookStore.Data;
using eBookStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace eBookStore.Controllers
{
    public class AccountController(IUserService service) : Controller
    {
        [HttpGet]
        public IActionResult Login(string errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage; // Set the error message if it's passed in the query string.
            }
            return View(); // This will render the Login.cshtml page in Views/Account folder.
        }

        [HttpPost]
        public IActionResult Login(Login loginModel)
        {
            // Check if the model is valid (all required fields are filled)
            if (ModelState.IsValid)
            {
                var user = service.GetUserById(loginModel.Email);

                if (user == null || user.Password != loginModel.Password)
                {
                    ViewBag.ErrorMessage = "Invalid email or password.";
                    return View();
                }

                // Here, you can store user info in session or cookies
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserID", user.UserID.ToString());

                // Redirect based on user role
                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (user.Role == "Customer")
                {
                    return RedirectToAction("Books", "Customer");
                }
            }

            // If model is not valid or login fails, redisplay the form
            return View();
        }

        // GET Signup page
        [HttpGet]
        public IActionResult Signup()
        {
            return View(); // This will render the Signup.cshtml page in Views/Account folder.
        }

        // POST Signup action
        [HttpPost]
        public IActionResult Signup(Signup signupModel)
        {
            // Check if the passwords match
            if (signupModel.Password != signupModel.ConfirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match."; // Set error message
                return View();
            }

            // Add logic to save user data to the database (dummy example here).

            return RedirectToAction("Login"); // Redirect to Login after successful signup.
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Clear session on logout
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
