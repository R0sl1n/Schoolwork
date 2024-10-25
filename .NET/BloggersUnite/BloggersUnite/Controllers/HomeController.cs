using Microsoft.AspNetCore.Mvc;
using BloggersUnite.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BloggersUnite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(IBlogRepository blogRepository, UserManager<IdentityUser> userManager)
        {
            _blogRepository = blogRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Hvis brukeren ikke er logget inn, returner en velkomstside
                return View("Welcome");
            }

            // Hvis brukeren er logget inn, vis bloggene
            var blogs = _blogRepository.GetAllBlogs();
            return View(blogs);
        }
    }
}