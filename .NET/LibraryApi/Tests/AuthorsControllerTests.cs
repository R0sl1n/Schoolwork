using LibraryApi.Controllers;
using LibraryApi.Services;
using LibraryApi.Models;
using LibraryApi.Dtos;
using Microsoft.AspNetCore.Mvc;


namespace Tests
{
    public class AuthorsControllerTests
    {
        private readonly AuthorService _authorService;
        private readonly AuthorsController _controller;

        // Constructor to set up the controller and service for testing
        public AuthorsControllerTests()
        {
            _authorService = MockHelpers.MockAuthorService();
            _controller = new AuthorsController(_authorService);
        }

        [Fact]
        // Test to verify that GetAuthors returns a list of authors with status OK
        public async Task GetAuthors_ReturnsOkResult_WithListOfAuthors()
        {
            var result = await _controller.GetAuthors();
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            var authors = okResult?.Value as IEnumerable<AuthorDto>;
            Assert.NotNull(authors);
        }

        [Fact]
        // Test to verify that GetAuthor returns NotFound when an author with the specified ID does not exist
        public async Task GetAuthor_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            var result = await _controller.GetAuthor(999);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        // Test to verify that PostAuthor adds a new author successfully
        public async Task PostAuthor_AddsAuthorSuccessfully()
        {
            var newAuthor = new Author { Id = 3, FirstName = "Jane", LastName = "Doe" };
            var result = await _controller.PostAuthor(newAuthor);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.NotNull(createdResult);
            var createdAuthor = createdResult!.Value as Author;
            Assert.NotNull(createdAuthor);
            Assert.Equal(newAuthor.FirstName, createdAuthor!.FirstName);
        }

        [Fact]
        // Test to verify that DeleteAuthor returns NotFound when attempting to delete a non-existent author
        public async Task DeleteAuthor_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            var result = await _controller.DeleteAuthor(999);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        // Test to verify that PutAuthor returns BadRequest when the IDs do not match
        public async Task PutAuthor_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            var author = new Author { Id = 2, FirstName = "Test", LastName = "Author" };
            var result = await _controller.PutAuthor(1, author);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        // Test to verify that PutAuthor returns NotFound when updating a non-existent author
        public async Task PutAuthor_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            // Arrange
            var nonExistentAuthor = new Author { Id = 99, FirstName = "NonExistent", LastName = "Author" };

            // Act
            var result = await _controller.PutAuthor(99, nonExistentAuthor);

            // Assert
            var notFoundResult = result as NotFoundResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult!.StatusCode);
        }

        [Fact]
        // Test to verify that CheckAuthor returns NotFound when the author with the specified name does not exist
        public async Task CheckAuthor_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            // Act
            var result = await _controller.CheckAuthor("Nonexistent", "Author");

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            Assert.NotNull(notFoundResult);
        }

        [Fact]
        // Test to verify that DeleteAuthor returns NoContent when deleting an existing author
        public async Task DeleteAuthor_ReturnsNoContent_WhenAuthorExists()
        {
            // Arrange
            var author = new Author { Id = 2, FirstName = "Jane", LastName = "Smith" };
            await _authorService.AddAuthorAsync(author);

            // Act
            var result = await _controller.DeleteAuthor(2);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult!.StatusCode);
        }

        [Fact]
        // Test to verify that PostAuthor returns Conflict when an author with the same name already exists
        public async Task PostAuthor_ReturnsConflict_WhenAuthorAlreadyExists()
        {
            var author = new Author { FirstName = "Duplicate", LastName = "Author" };
            await _authorService.AddAuthorAsync(author);
            var duplicateAuthor = new Author { FirstName = "Duplicate", LastName = "Author" };
            var result = await _controller.PostAuthor(duplicateAuthor);
            var conflictResult = result.Result as ConflictObjectResult;
            Assert.NotNull(conflictResult);
            Assert.Equal(409, conflictResult!.StatusCode);
        }

        [Fact]
        // Test to verify that PutAuthor returns NoContent when updating an existing author
        public async Task PutAuthor_ReturnsNoContent_WhenAuthorIsUpdated()
        {
            var author = new Author { Id = 1, FirstName = "Updated", LastName = "Author" };
            await _authorService.AddAuthorAsync(author);
            author.FirstName = "Changed";
            var result = await _controller.PutAuthor(1, author);
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult!.StatusCode);
        }

    }
}
