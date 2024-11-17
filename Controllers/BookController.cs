using eBookStore.Models;
using eBookStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace eBookStore.Controllers;

public class BookController(IBookService service) : Controller
{
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(Book book)
    {
        if (!ModelState.IsValid)
        {
            return View(book);
        }

        var result = service.AddBook(book);

        if (result)
        {
            TempData["msg"] = "Book Added Successfully !";
            return RedirectToAction(nameof(Add));
        }

        TempData["error"] = "Error has occured on server side";
        return View(book);
    }

    public IActionResult Update(int id)
    {
        var book = service.GetBookById(id);
        return View(book);
    }

    [HttpPost]
    public IActionResult Update(Book book)
    {
        if (!ModelState.IsValid)
        {
            return View(book);
        }

        var result = service.UpdateBook(book);

        if (result)
        {
            TempData["msg"] = "Book Added Successfully !";
            return RedirectToAction(nameof(Add));
        }

        TempData["error"] = "Error has occured on server side";
        return View(book);
    }

    public IActionResult Delete(int id)
    {
        var result = service.DeleteBook(id);
        return RedirectToAction("GetAll");
    }

    public IActionResult GetAll()
    {
        var data = service.GetAllBooks();
        return View(data);
    }
}