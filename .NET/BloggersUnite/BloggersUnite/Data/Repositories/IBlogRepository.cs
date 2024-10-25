using BloggersUnite.Models;

namespace BloggersUnite.Data.Repositories.Interfaces
{
    public interface IBlogRepository
    {
        // Hent alle blogger
        IEnumerable<Blog> GetAllBlogs();

        // Hent en blogg basert på id
        Blog GetBlogById(int id);

        // Opprett en ny blogg
        void CreateBlog(Blog blog);

        // Oppdater en eksisterende blogg
        void UpdateBlog(Blog blog);

        // Slett en blogg og alle tilhørende poster og kommentarer
        void DeleteBlog(Blog blog);

        // Lagre endringer til databasen
        void Save();
    }
}