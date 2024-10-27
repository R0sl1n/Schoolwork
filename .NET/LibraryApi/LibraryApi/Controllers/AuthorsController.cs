using Microsoft.AspNetCore.Mvc;
using LibraryApi.Services;
using LibraryApi.Dtos;
using LibraryApi.Models;

namespace LibraryApi.Controllers
{
    // API Controller for managing authors456
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorService _authorService;

        // Constructor to initialize the AuthorService dependency
        public AuthorsController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: api/authors
        // Returns a list of all authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            return Ok(authors);
        }

        // GET: api/authors/{id}
        // Returns a specific author by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null) return NotFound(); // Returns 404 if the author does not exist
            return Ok(author); // Returns 200 with the author data if found
        }

        // GET: api/authors/check?firstName={firstName}&lastName={lastName}
        // Checks if an author with the specified name exists
        [HttpGet("check")]
        public async Task<ActionResult<AuthorDto?>> CheckAuthor([FromQuery] string firstName, [FromQuery] string lastName)
        {
            var author = await _authorService.GetAuthorByNameAsync(firstName, lastName);
            if (author == null) return NotFound(); // Returns 404 if the author does not exist
            return Ok(author); // Returns 200 with the author data if found
        }

        // POST: api/authors
        // Adds a new author
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> PostAuthor(Author author)
        {
            try
            {
                var createdAuthor = await _authorService.AddAuthorAsync(author);
                // Returns 201 with the location of the newly created author
                return CreatedAtAction(nameof(GetAuthor), new { id = createdAuthor.Id }, createdAuthor);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // Returns 409 Conflict if the author already exists
            }
        }


        // PUT: api/authors/{id}
        // Updates an existing author by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.Id) return BadRequest(); // Returns 400 if the IDs do not match

            var success = await _authorService.UpdateAuthorAsync(author);
            if (!success) return NotFound(); // Returns 404 if the author does not exist

            return NoContent(); // Returns 204 if the update is successful
        }

        // DELETE: api/authors/{id}
        // Deletes an author by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var success = await _authorService.DeleteAuthorAsync(id);
            if (!success) return NotFound(); // Returns 404 if the author does not exist

            return NoContent(); // Returns 204 if the deletion is successful
        }
    }
}
