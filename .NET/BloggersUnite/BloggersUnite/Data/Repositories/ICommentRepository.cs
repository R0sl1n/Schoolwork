using BloggersUnite.Models;
using System.Collections.Generic;

namespace BloggersUnite.Data.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        IEnumerable<Comment> GetAllComments();
        IEnumerable<Comment> GetCommentsByPostId(int postId);
        Comment GetCommentById(int id);
        Comment GetCommentByIdAsNoTracking(int id); // Hent kommentar uten tracking
        void CreateComment(Comment comment);
        void UpdateComment(Comment comment);
        void DeleteComment(Comment comment);
        void Save();
    }
}