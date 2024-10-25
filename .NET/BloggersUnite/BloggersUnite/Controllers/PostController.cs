using BloggersUnite.Data.Repositories;
using BloggersUnite.Data.Repositories.Interfaces;
using BloggersUnite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class PostController : Controller
{
    private readonly IPostRepository _postRepository;
    private readonly IBlogRepository _blogRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public PostController(IPostRepository postRepository, IBlogRepository blogRepository, ICommentRepository commentRepository, UserManager<IdentityUser> userManager)
    {
        _postRepository = postRepository;
        _blogRepository = blogRepository;
        _commentRepository = commentRepository;
        _userManager = userManager;
    }

    // GET: Post/Details/5
    public IActionResult Details(int id)
    {
        // Bruker GetPostByIdWithBlog for å inkludere Blog og Comments når vi henter posten
        var post = _postRepository.GetPostByIdWithBlog(id);

        if (post == null)
        {
            return NotFound();
        }

        // Gå gjennom alle kommentarer og hent e-posten til hver eier
        foreach (var comment in post.Comments)
        {
            if (!string.IsNullOrEmpty(comment.OwnerId))
            {
                var owner = _userManager.FindByIdAsync(comment.OwnerId).Result;
                if (owner != null)
                {
                    comment.Owner.Email = owner.Email; // Sett e-postadressen til Owner-objektet
                }
            }
        }

        return View(post);
    }


    // POST: Post/CreateComment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateComment(int postId, string content)
    {
        var post = _postRepository.GetPostById(postId);
        var blog = _blogRepository.GetBlogById(post.BlogId);

        if (post == null || blog == null || !blog.IsOpen)
        {
            return Forbid(); // Prevent adding comments if the blog is closed for comments
        }

        var comment = new Comment
        {
            Content = content,
            PostId = postId,
            OwnerId = _userManager.GetUserId(User),  // Set the owner to the logged-in user
            CreatedOn = DateTime.Now
        };

        _commentRepository.CreateComment(comment);
        _commentRepository.Save();

        // After the comment is created, redirect back to the post details
        return RedirectToAction("Details", new { id = postId });
    }

    // GET: Post/Create
    public IActionResult Create(int blogId)
    {
        var blog = _blogRepository.GetBlogById(blogId);

        if (blog == null || !blog.AllowPosts)
        {
            return NotFound();
        }

        // Sjekk om innlogget bruker er eieren av bloggen
        if (_userManager.GetUserId(User) != blog.OwnerId)
        {
            return Forbid();
        }

        var post = new Post
        {
            BlogId = blogId
        };
        return View(post);
    }

    // POST: Post/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Post post)
    {
        if (!ModelState.IsValid)
        {
            // Log ModelState errors to console for debugging
            Console.WriteLine("ModelState is not valid");
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                }
            }
        }

        if (ModelState.IsValid)
        {
            var blog = _blogRepository.GetBlogById(post.BlogId);
            if (blog == null)
            {
                ModelState.AddModelError("", "Blog not found.");
                return View(post);
            }

            // Check if posts are allowed on this blog
            if (!blog.AllowPosts)
            {
                return Forbid(); // Prevent creating a post if the blog does not allow new posts
            }

            if (_userManager.GetUserId(User) != blog.OwnerId)
            {
                return Forbid();
            }

            // Ensure OwnerId is set in the controller
            post.OwnerId = _userManager.GetUserId(User);

            _postRepository.CreatePost(post);
            _postRepository.Save();

            return RedirectToAction("Details", "Blog", new { id = post.BlogId });
        }

        return View(post);
    }


    // GET: Post/Edit/5
    public IActionResult Edit(int id)
    {
        var post = _postRepository.GetPostById(id);
        if (post == null)
        {
            return NotFound();
        }

        // Sjekk om innlogget bruker er eieren av posten
        if (_userManager.GetUserId(User) != post.OwnerId)
        {
            return Forbid();
        }

        return View(post);
    }

    // POST: Post/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Post post)
    {
        if (ModelState.IsValid)
        {
            var existingPost = _postRepository.GetPostById(post.Id);

            // Sjekk om posten eksisterer og om innlogget bruker er eieren av posten
            if (existingPost == null || _userManager.GetUserId(User) != existingPost.OwnerId)
            {
                return Forbid();
            }

            post.OwnerId = existingPost.OwnerId; // Behold eieren
            _postRepository.UpdatePost(post);
            return RedirectToAction("Details", "Blog", new { id = post.BlogId });
        }
        return View(post);
    }

    // GET: Post/Delete/5
    public IActionResult Delete(int id)
    {
        var post = _postRepository.GetPostById(id);
        if (post == null)
        {
            return NotFound();
        }

        // Sjekk om innlogget bruker er eieren av posten
        if (_userManager.GetUserId(User) != post.OwnerId)
        {
            return Forbid();
        }

        return View(post);
    }

    // POST: Post/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var post = _postRepository.GetPostById(id);
        if (post == null)
        {
            return NotFound();
        }

        // Sjekk om innlogget bruker er eieren av posten
        if (_userManager.GetUserId(User) != post.OwnerId)
        {
            return Forbid();
        }

        _postRepository.DeletePost(post);
        _postRepository.Save();
        return RedirectToAction("Details", "Blog", new { id = post.BlogId });
    }
}
