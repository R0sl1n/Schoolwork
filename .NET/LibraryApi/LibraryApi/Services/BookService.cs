using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Services
{
    // Service class for managing book-related operations in the application
    public class BookService
    {
        private readonly LibraryDbContext _context;

        // Constructor that initializes the service with the database context
        public BookService(LibraryDbContext context)
        {
            _context = context;
        }

        // Retrieves all books from the database, including their author and category information, and maps them to DTOs
        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();

            return books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Year = b.Year,
                AuthorName = $"{b.Author?.FirstName} {b.Author?.LastName}",
                CategoryName = b.Category?.Name ?? "Uncategorized"
            });
        }

        // Retrieves a specific book by its ID and maps it to a DTO
        public async Task<BookDto?> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return null;

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Year = book.Year,
                AuthorName = $"{book.Author?.FirstName} {book.Author?.LastName}",
                CategoryName = book.Category?.Name ?? "Uncategorized"
            };
        }

        // Retrieves a book by title, author ID, and year
        public async Task<Book?> GetBookByDetailsAsync(string title, int authorId, int year)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Title == title && b.AuthorId == authorId && b.Year == year);
        }

        // Adds a new book to the database
        public async Task<Book> AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        // Updates an existing book in the database based on the provided DTO
        public async Task<bool> UpdateBookAsync(BookDto bookDto)
        {
            var existingBook = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == bookDto.Id);

            if (existingBook == null)
            {
                return false;
            }

            // Update the book's fields
            existingBook.Title = bookDto.Title;
            existingBook.Description = bookDto.Description;
            existingBook.Year = bookDto.Year;

            // Update the author if necessary
            var author = await _context.Authors
                .FirstOrDefaultAsync(a => a.FirstName + " " + a.LastName == bookDto.AuthorName);
            if (author == null)
            {
                var nameParts = bookDto.AuthorName.Split(' ');
                author = new Author
                {
                    FirstName = nameParts[0],
                    LastName = nameParts[1]
                };
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
            }
            existingBook.AuthorId = author.Id;

            // Update the category if necessary
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == bookDto.CategoryName);
            if (category == null)
            {
                category = new Category
                {
                    Name = bookDto.CategoryName
                };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            existingBook.CategoryId = category.Id;

            _context.Entry(existingBook).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        // Deletes a book from the database based on its ID
        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        // Adds a book based on the provided DTO, ensuring the author and category exist or are created
        public async Task<Book> AddBookAsync(BookDto bookDto)
        {
            // Check if the author exists based on the name (combination of first and last name)
            var author = await _context.Authors
                .FirstOrDefaultAsync(a => a.FirstName + " " + a.LastName == bookDto.AuthorName);
            if (author == null)
            {
                var nameParts = bookDto.AuthorName.Split(' ');
                if (nameParts.Length < 2)
                {
                    throw new ArgumentException("Full author name (first and last) is required.");
                }

                author = new Author
                {
                    FirstName = nameParts[0],
                    LastName = nameParts[1]
                };
                _context.Authors.Add(author);
                await _context.SaveChangesAsync(); // Save to assign ID
            }

            // Check if the category exists based on the name
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == bookDto.CategoryName);
            if (category == null)
            {
                category = new Category
                {
                    Name = bookDto.CategoryName
                };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync(); // Save to assign ID
            }

            // Check if the book already exists based on title, author, and year
            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.Title == bookDto.Title && b.AuthorId == author.Id && b.Year == bookDto.Year);
            if (existingBook != null)
            {
                throw new InvalidOperationException("A book with the same title, author, and year already exists.");
            }

            // Create a new book with references to the author and category
            var book = new Book
            {
                Title = bookDto.Title,
                Description = bookDto.Description,
                Year = bookDto.Year,
                AuthorId = author.Id,
                CategoryId = category.Id
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync(); // Save at the end
            return book;
        }
    }
}
