using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Services
{
    // Service class for managing author-related operations in the application
    public class AuthorService
    {
        private readonly LibraryDbContext _context;

        // Constructor that initializes the service with the database context
        public AuthorService(LibraryDbContext context)
        {
            _context = context;
        }

        // Retrieves all authors from the database, including their books, and maps them to DTOs
        public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
        {
            var authors = await _context.Authors.Include(a => a.Books).ToListAsync();
            return authors.Select(a => new AuthorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                BookTitles = a.Books?.Select(b => b.Title).ToList() ?? new List<string>()
            });
        }

        // Retrieves a specific author by their ID and maps them to a DTO
        public async Task<AuthorDto?> GetAuthorByIdAsync(int id)
        {
            var author = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
            if (author == null) return null;

            return new AuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                BookTitles = author.Books?.Select(b => b.Title).ToList() ?? new List<string>()
            };
        }

        // Retrieves an author based on their first and last name
        public async Task<Author?> GetAuthorByNameAsync(string firstName, string lastName)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.FirstName == firstName && a.LastName == lastName);
        }

        // Adds a new author to the database, ensuring no duplicate authors exist
        public async Task<Author> AddAuthorAsync(Author author)
        {
            // Check if an author with the same first and last name already exists (case-insensitive check)
            var existingAuthor = await _context.Authors
                .FirstOrDefaultAsync(a => a.FirstName.ToLower() == author.FirstName.ToLower() &&
                                          a.LastName.ToLower() == author.LastName.ToLower());

            if (existingAuthor != null)
            {
                throw new InvalidOperationException("An author with the same name already exists.");
            }

            // Add the new author if no existing author was found
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }


        // Updates an existing author's information in the database
        public async Task<bool> UpdateAuthorAsync(Author author)
        {
            if (!_context.Authors.Any(a => a.Id == author.Id))
            {
                return false;
            }

            _context.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        // Deletes an author from the database based on their ID
        public async Task<bool> DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return false;
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
