using BloggersUnite.Models;
using Microsoft.EntityFrameworkCore;

namespace BloggersUnite.Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Fetch all posts from the database
        public IEnumerable<Post> GetAllPosts()
        {
            return _context.Posts.ToList();
        }

        // Fetch posts by blog ID
        public IEnumerable<Post> GetPostsByBlogId(int blogId)
        {
            return _context.Posts
                .Where(p => p.BlogId == blogId)
                .ToList();
        }

        // Fetch post by ID
        public Post GetPostById(int id)
        {
            return _context.Posts
                .Include(p => p.Comments) // Inkluderer kommentarer
                .FirstOrDefault(p => p.Id == id);
        }

        // Ny metode for å hente post med tilhørende blog
        public Post GetPostByIdWithBlog(int id)
        {
            return _context.Posts
                .Include(p => p.Blog)     // Inkluderer bloggen til posten
                .Include(p => p.Comments) // Inkluderer kommentarer
                .FirstOrDefault(p => p.Id == id);
        }

        // Create a new post
        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            Save();
        }

        // Update an existing post
        public void UpdatePost(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            var existingPost = _context.Posts.AsNoTracking().FirstOrDefault(p => p.Id == post.Id);

            if (existingPost != null)
            {
                var trackedPost = _context.Posts.Local.FirstOrDefault(p => p.Id == post.Id);
                if (trackedPost != null)
                {
                    _context.Entry(trackedPost).State = EntityState.Detached;
                }

                _context.Posts.Attach(post);
                _context.Entry(post).State = EntityState.Modified;
                Save();
            }
            else
            {
                throw new KeyNotFoundException("Post not found.");
            }
        }

        // Delete a post
        public void DeletePost(Post post)
        {
            var existingPost = _context.Posts.Find(post.Id);
            if (existingPost != null)
            {
                _context.Posts.Remove(existingPost);
            }
        }

        // Save changes to the database
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
