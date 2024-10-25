using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Books");
        }
    }
}