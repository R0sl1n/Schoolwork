using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(LibraryDbContext context)
        {
            context.Database.Migrate();

            // Seed authors if none exist
            if (!context.Authors.Any())
            {
                var authors = new[]
                {
                    new Author { Id = 1, FirstName = "George", LastName = "Orwell" },
                    new Author { Id = 2, FirstName = "Jane", LastName = "Austen" }
                };
                context.Authors.AddRange(authors);
                context.SaveChanges();
            }

            // Seed categories if none exist
            if (!context.Categories.Any())
            {
                var categories = new[]
                {
                    new Category { Id = 1, Name = "Fiction" },
                    new Category { Id = 2, Name = "Classics" }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            // Seed books if none exist
            if (!context.Books.Any())
            {
                var books = new[]
                {
                    new Book { Id = 1, Title = "1984", Description = "Dystopian novel", Year = 1949, AuthorId = 1, CategoryId = 1 },
                    new Book { Id = 2, Title = "Pride and Prejudice", Description = "Romantic novel", Year = 1813, AuthorId = 2, CategoryId = 2 }
                };
                context.Books.AddRange(books);
                context.SaveChanges();
            }
        }
    }
}