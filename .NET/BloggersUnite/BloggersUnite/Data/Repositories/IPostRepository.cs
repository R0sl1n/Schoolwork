using BloggersUnite.Models;

namespace BloggersUnite.Data.Repositories
{
    public interface IPostRepository
    {
        // Hent alle innlegg (posts)
        IEnumerable<Post> GetAllPosts();

        // Hent innlegg basert på blogId
        IEnumerable<Post> GetPostsByBlogId(int blogId);

        // Hent et innlegg basert på id
        Post GetPostById(int id);

        // Hent innlegg og tilhørende blog basert på id
        Post GetPostByIdWithBlog(int id); 

        // Opprett et nytt innlegg
        void CreatePost(Post post);

        // Oppdater et eksisterende innlegg
        void UpdatePost(Post post);

        // Slett et innlegg
        void DeletePost(Post post);

        // Lagre endringer til databasen
        void Save();
    }
}