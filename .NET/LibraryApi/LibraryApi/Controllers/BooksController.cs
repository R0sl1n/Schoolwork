using Microsoft.AspNetCore.Mvc;
using LibraryApi.Services;
using LibraryApi.Dtos;
using LibraryApi.Models;

namespace LibraryApi.Controllers
{
    // API Controller for managing books
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        // Constructor to initialize the BookService dependency
        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/books
        // Returns a list of all books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books); // Returns 200 OK with the list of books
        }

        // GET: api/books/{id}
        // Returns a specific book by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound(); // Returns 404 if the book is not found
            return Ok(book); // Returns 200 OK with the book details
        }

        // POST: api/books
        // Adds a new book
        [HttpPost]
        public async Task<ActionResult<BookDto>> PostBook(BookDto bookDto)
        {
            try
            {
                var createdBook = await _bookService.AddBookAsync(bookDto);
                // Returns 201 Created with the location of the newly created book
                return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
            }
            catch (ArgumentException ex)
            {
                // Returns 400 Bad Request if the book data is invalid
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Returns 409 Conflict if there is a conflict (e.g., book already exists)
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Returns 500 Internal Server Error for any other exceptions
                return StatusCode(500, new { message = $"An error occurred while saving the book: {ex.Message}" });
            }
        }

        // PUT: api/books/{id}
        // Updates an existing book by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, [FromBody] BookDto bookDto)
        {
            if (id != bookDto.Id)
            {
                // Returns 400 Bad Request if the provided ID does not match the book's ID
                return BadRequest("The book ID does not match.");
            }

            try
            {
                var updated = await _bookService.UpdateBookAsync(bookDto);
                if (!updated)
                {
                    // Returns 404 Not Found if the book does not exist
                    return NotFound("Book not found.");
                }
                // Returns 204 No Content if the update is successful
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // Returns 400 Bad Request if there is an error specific to the update process
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/books/{id}
        // Deletes a book by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var success = await _bookService.DeleteBookAsync(id);
            if (!success) return NotFound(); // Returns 404 if the book does not exist

            // Returns 204 No Content if the deletion is successful
            return NoContent();
        }

        // GET: api/books/details?title={title}&authorId={authorId}&year={year}
        // Gets a book by specific details: title, authorId, and year
        [HttpGet("details")]
        public async Task<ActionResult<Book>> GetBookByDetails([FromQuery] string title, [FromQuery] int authorId, [FromQuery] int year)
        {
            var book = await _bookService.GetBookByDetailsAsync(title, authorId, year);
            if (book == null) return NotFound(); // Returns 404 if no matching book is found
            return Ok(book); // Returns 200 OK with the book details if found
        }
    }
}
