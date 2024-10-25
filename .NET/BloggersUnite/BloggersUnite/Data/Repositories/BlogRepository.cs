using BloggersUnite.Data.Repositories.Interfaces;
using BloggersUnite.Models;
using Microsoft.EntityFrameworkCore;

namespace BloggersUnite.Data.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Blog> GetAllBlogs()
        {
            return _context.Blogs.ToList();
        }

        public Blog GetBlogById(int id)
        {
            return _context.Blogs
                .Include(b => b.Posts) // Load related posts
                .FirstOrDefault(b => b.Id == id);
        }

        public void CreateBlog(Blog blog)
        {
            if (blog == null)
            {
                throw new ArgumentNullException(nameof(blog));
            }
            _context.Blogs.Add(blog);
            Save();
        }

        public void UpdateBlog(Blog blog)
        {
            if (blog == null)
            {
                throw new ArgumentNullException(nameof(blog));
            }

            // Retrieve the original blog from the database with no tracking
            var existingBlog = _context.Blogs.AsNoTracking().FirstOrDefault(b => b.Id == blog.Id);

            if (existingBlog != null)
            {
                // Detach any tracked entity with the same key in the local context (if it exists)
                var trackedBlog = _context.Blogs.Local.FirstOrDefault(b => b.Id == blog.Id);
                if (trackedBlog != null)
                {
                    _context.Entry(trackedBlog).State = EntityState.Detached;
                }

                // Attach the updated blog entity and mark it as modified
                _context.Blogs.Attach(blog);
                _context.Entry(blog).State = EntityState.Modified;

                // Save changes to update the blog in the database
                Save();
            }
            else
            {
                throw new KeyNotFoundException("Blog not found.");
            }
        }


        public void DeleteBlog(Blog blog)
        {
            if (blog == null)
            {
                throw new ArgumentNullException(nameof(blog));
            }

            _context.Blogs.Remove(blog);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}