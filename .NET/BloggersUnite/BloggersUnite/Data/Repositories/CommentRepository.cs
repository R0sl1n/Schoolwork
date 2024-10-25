using BloggersUnite.Data.Repositories.Interfaces;
using BloggersUnite.Models;
using Microsoft.EntityFrameworkCore;

namespace BloggersUnite.Data.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Comment> GetCommentsByPostId(int postId)
        {
            return _context.Comments
                .Include(c => c.Post)
                .Include(c => c.Owner)
                .Where(c => c.PostId == postId)
                .ToList();
        }

        public Comment GetCommentById(int id)
        {
            return _context.Comments
                .Include(c => c.Post) // Inkluder relasjonen til Post
                .Include(c => c.Owner) // Inkluder relasjonen til Owner
                .FirstOrDefault(c => c.Id == id);
        }

        // Hent kommentar uten endringssporing
        public Comment GetCommentByIdAsNoTracking(int id)
        {
            return _context.Comments
                .AsNoTracking()
                .Include(c => c.Post)   // Sørg for at Post-relasjonen lastes inn
                .Include(c => c.Owner)  // Sørg for at Owner-relasjonen lastes inn
                .FirstOrDefault(c => c.Id == id);
        }



        public void CreateComment(Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            _context.Comments.Add(comment);
        }

        public void UpdateComment(Comment comment)
        {
            // Sjekk om kommentaren allerede spores av Entity Framework
            var trackedComment = _context.Comments.Local.FirstOrDefault(c => c.Id == comment.Id);

            if (trackedComment != null)
            {
                // Hvis den sporer kommentaren, frigjør den
                _context.Entry(trackedComment).State = EntityState.Detached;
            }

            // Koble til kommentaren
            _context.Comments.Attach(comment);

            // Marker nødvendige felter som modifisert
            _context.Entry(comment).Property(c => c.Content).IsModified = true;
            _context.Entry(comment).Property(c => c.CreatedOn).IsModified = true;

            // Lagre endringer
            Save();
        }



        public void DeleteComment(Comment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));

            var existingComment = _context.Comments.Find(comment.Id);
            if (existingComment == null)
            {
                throw new KeyNotFoundException("Comment not found");
            }

            _context.Comments.Remove(existingComment);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Comment> GetAllComments()
        {
            return _context.Comments.ToList();
        }
    }
}
