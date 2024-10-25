using BloggersUnite.Data.Repositories;
using BloggersUnite.Data.Repositories.Interfaces;
using BloggersUnite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BloggersUnite.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public BlogController(IBlogRepository blogRepository, IPostRepository postRepository, ICommentRepository commentRepository, UserManager<IdentityUser> userManager)
        {
            _blogRepository = blogRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _userManager = userManager;
        }

        // GET: Blog/Index
        public IActionResult Index()
        {
            var blogs = _blogRepository.GetAllBlogs();
            return View(blogs);
        }

        // GET: Blog/Details/5
        public IActionResult Details(int id)
        {
            var blog = _blogRepository.GetBlogById(id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blog/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Blog blog)
        {
            if (ModelState.IsValid)
            {
                blog.OwnerId = _userManager.GetUserId(User); // Assign the blog to the logged-in user
                _blogRepository.CreateBlog(blog);
                _blogRepository.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(blog);
        }

        // GET: Blog/Edit/5
        public IActionResult Edit(int id)
        {
            var blog = _blogRepository.GetBlogById(id);
            if (blog == null)
            {
                return NotFound();
            }

            // Check if the logged-in user is the owner of the blog
            if (_userManager.GetUserId(User) != blog.OwnerId)
            {
                return Forbid(); // User is not the owner
            }

            return View(blog);
        }

        // POST: Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Blog blog)
        {
            if (ModelState.IsValid)
            {
                var existingBlog = _blogRepository.GetBlogById(blog.Id);

                // Ensure that the OwnerId is preserved
                if (existingBlog == null || _userManager.GetUserId(User) != existingBlog.OwnerId)
                {
                    return Forbid();
                }

                // Set the OwnerId to prevent it from being cleared
                blog.OwnerId = existingBlog.OwnerId;

                _blogRepository.UpdateBlog(blog);
                _blogRepository.Save();
                return RedirectToAction(nameof(Index)); // Redirect back to the blog list
            }
            return View(blog);
        }

        // GET: Blog/Delete/5
        public IActionResult Delete(int id)
        {
            var blog = _blogRepository.GetBlogById(id);
            if (blog == null)
            {
                return NotFound();
            }

            // Check if the logged in user is the owner of the blog
            if (_userManager.GetUserId(User) != blog.OwnerId)
            {
                return Forbid(); 
            }

            return View(blog);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var blog = _blogRepository.GetBlogById(id);
            if (blog == null)
            {
                return NotFound();
            }

            // Check if the logged-in user is the owner of the blog
            if (_userManager.GetUserId(User) != blog.OwnerId)
            {
                return Forbid(); // User is not the owner
            }

            // Delete all posts and comments associated with the blog
            var posts = _postRepository.GetPostsByBlogId(id);
            foreach (var post in posts)
            {
                var comments = _commentRepository.GetCommentsByPostId(post.Id);
                foreach (var comment in comments)
                {
                    _commentRepository.DeleteComment(comment);
                }
                _postRepository.DeletePost(post);
            }

            // Delete the blog
            _blogRepository.DeleteBlog(blog);
            _blogRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View("SearchResults", new List<Blog>());
            }

            var matchingBlogs = _blogRepository.GetAllBlogs()
                .Where(b => (b.Title != null && b.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
                            || (b.Description != null && b.Description.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return View("SearchResults", matchingBlogs);
        }

    }
}
