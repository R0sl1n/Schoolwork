using Microsoft.AspNetCore.Identity;
using BloggersUnite.Data;

namespace BloggersUnite.Models
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Seeding brukere
            var user1 = new IdentityUser
            {
                UserName = "user1@bloggersunite.com",
                Email = "user1@bloggersunite.com",
                EmailConfirmed = true
            };

            var user2 = new IdentityUser
            {
                UserName = "user2@bloggersunite.com",
                Email = "user2@bloggersunite.com",
                EmailConfirmed = true
            };

            var password1 = "User1Password123!";
            var password2 = "User2Password123!";

            if (await userManager.FindByEmailAsync(user1.Email) == null)
            {
                await userManager.CreateAsync(user1, password1);
            }

            if (await userManager.FindByEmailAsync(user2.Email) == null)
            {
                await userManager.CreateAsync(user2, password2);
            }

            // Oppdater brukere for å hente deres Id-er etter at de er opprettet
            user1 = await userManager.FindByEmailAsync(user1.Email);
            user2 = await userManager.FindByEmailAsync(user2.Email);

            // Seeding blogger, innlegg og kommentarer
            SeedBlogsAndPosts(context, user1, user2);

            await context.SaveChangesAsync();
        }

        private static void SeedBlogsAndPosts(ApplicationDbContext context, IdentityUser user1, IdentityUser user2)
        {
            // Sjekk om blogger allerede er seeded
            if (!context.Blogs.Any())
            {
                var blog1 = new Blog
                {
                    Title = "User1's Blog",
                    Description = "This is User1's first blog.",
                    OwnerId = user1.Id,
                    IsOpen = true,
                    AllowPosts = true
                };

                var blog2 = new Blog
                {
                    Title = "User2's Blog",
                    Description = "This is User2's first blog.",
                    OwnerId = user2.Id,
                    IsOpen = true,
                    AllowPosts = true
                };

                context.Blogs.AddRange(blog1, blog2);

                // Lagre blogger først for å generere BlogId
                context.SaveChanges();

                // Opprett innlegg (posts) for hver blogg
                var post1 = new Post
                {
                    Title = "First Post in User1's Blog",
                    Content = "This is the first post in User1's blog.",
                    BlogId = blog1.Id, // Bruk generert BlogId
                    OwnerId = user1.Id
                };

                var post2 = new Post
                {
                    Title = "First Post in User2's Blog",
                    Content = "This is the first post in User2's blog.",
                    BlogId = blog2.Id, // Bruk generert BlogId
                    OwnerId = user2.Id
                };

                context.Posts.AddRange(post1, post2);

                // Lagre innleggdata først for å generere PostId
                context.SaveChanges();

                // Opprett kommentarer etter at postene er lagret
                var comment1 = new Comment
                {
                    Content = "This is a comment on User1's post.",
                    PostId = post1.Id,  // Bruk gyldig PostId
                    OwnerId = user2.Id  // Kommentar skrevet av user2
                };

                var comment2 = new Comment
                {
                    Content = "This is a comment on User2's post.",
                    PostId = post2.Id,  // Bruk gyldig PostId
                    OwnerId = user1.Id  // Kommentar skrevet av user1
                };

                context.Comments.AddRange(comment1, comment2);

                // Lagre kommentarene til slutt
                context.SaveChanges();
            }
        }
    }
}
