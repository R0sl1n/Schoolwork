using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    public class BooksController : Controller
    {
        // Index action to load the main book page
        public IActionResult Index()
        {
            return View();
        }

        // AddOrEdit action to load the form for adding/updating a book
        public IActionResult AddOrEdit(int? id)
        {
            ViewBag.BookId = id;
            return View();
        }
    }
}