using eBookStore.Models;
using eBookStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace eBookStore.Controllers
{
    public class AdminController(IUserService service) : Controller
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
                return View("User");
            }

            ViewBag.ErrorMessage = "Error occured please try again !";
            return View("User");
        }
    }
}
