using LibraryApi.Controllers;
using LibraryApi.Services;
using LibraryApi.Models;
using LibraryApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class BooksControllerTests
    {
        private readonly BookService _bookService;
        private readonly BooksController _controller;

        // Constructor to set up the controller and service for testing
        public BooksControllerTests()
        {
            _bookService = MockHelpers.MockBookService();
            _controller = new BooksController(_bookService);
        }

        [Fact]
        public async Task GetBooks_ReturnsOkResult_WithListOfBooks()
        {
            var result = await _controller.GetBooks();
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            if (okResult?.Value is IEnumerable<BookDto> books)
            {
                Assert.NotNull(books);
            }
        }

        [Fact]
        public async Task GetBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var result = await _controller.GetBook(999);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostBook_AddsBookSuccessfully()
        {
            var newBookDto = new BookDto { Title = "New Book", Year = 2021, AuthorName = "Test Author", CategoryName = "Test Category" };
            var result = await _controller.PostBook(newBookDto);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.NotNull(createdResult);
            var createdBook = createdResult?.Value as Book;
            Assert.NotNull(createdBook);
            Assert.Equal(newBookDto.Title, createdBook?.Title);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var result = await _controller.DeleteBook(999);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutBook_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            var bookDto = new BookDto { Id = 2, Title = "Updated Title" };
            var result = await _controller.PutBook(1, bookDto);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetBooks_ReturnsEmptyList_WhenNoBooksExist()
        {
            var result = await _controller.GetBooks();
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            var books = okResult?.Value as IEnumerable<BookDto>;
            Assert.NotNull(books);
            Assert.Empty(books);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNoContent_WhenBookExists()
        {
            var bookDto = new BookDto { Id = 1, Title = "Existing Book", AuthorName = "Test Author", Year = 2022 };
            await _bookService.AddBookAsync(bookDto);

            var result = await _controller.DeleteBook(1);
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult!.StatusCode);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var bookDto = new BookDto { Id = 999, Title = "Nonexistent Book" };
            var result = await _controller.PutBook(999, bookDto);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetBookByDetails_ReturnsNotFound_WhenDetailsDoNotMatch()
        {
            var result = await _controller.GetBookByDetails("Nonexistent Title", 999, 1999);
            var notFoundResult = result.Result as NotFoundResult;
            Assert.NotNull(notFoundResult);
        }

        [Fact]
        public async Task PostBook_ReturnsBadRequest_WhenInvalidData()
        {
            var invalidBookDto = new BookDto { Title = "", Year = -1, AuthorName = "", CategoryName = "" };
            var result = await _controller.PostBook(invalidBookDto);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult!.StatusCode);
        }

        [Fact]
        public async Task PutBook_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            var bookDto = new BookDto { Id = 1, Title = "Updated Book", Year = 2022, AuthorName = "Test Author", CategoryName = "Test Category" };
            await _bookService.AddBookAsync(bookDto);

            var result = await _controller.PutBook(1, bookDto);
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PostBook_ReturnsConflict_WhenBookAlreadyExists()
        {
            var existingBookDto = new BookDto { Id = 1, Title = "Existing Book", Year = 2022, AuthorName = "Test Author", CategoryName = "Test Category" };
            await _bookService.AddBookAsync(existingBookDto);

            var newBookDto = new BookDto { Id = 2, Title = "Existing Book", Year = 2022, AuthorName = "Test Author", CategoryName = "Test Category" };
            var result = await _controller.PostBook(newBookDto);

            var conflictResult = result.Result as ConflictObjectResult;
            Assert.NotNull(conflictResult);
            Assert.Equal(409, conflictResult!.StatusCode);
        }

        [Fact]
        public async Task GetBookByDetails_ReturnsCorrectBook_WhenDetailsMatch()
        {
            var bookDto = new BookDto { Id = 1, Title = "Unique Title", Year = 2023, AuthorName = "Specific Author", CategoryName = "Fiction" };
            await _bookService.AddBookAsync(bookDto);

            var result = await _controller.GetBookByDetails("Unique Title", 1, 2023);
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            var foundBook = okResult?.Value as Book;
            Assert.NotNull(foundBook);
            Assert.Equal("Unique Title", foundBook?.Title);
        }

        [Fact]
        public async Task GetBooks_ReturnsCorrectNumberOfBooks_WhenMultipleBooksExist()
        {
            var book1 = new BookDto { Id = 1, Title = "First Book", Year = 2021, AuthorName = "Author One", CategoryName = "Category One" };
            var book2 = new BookDto { Id = 2, Title = "Second Book", Year = 2022, AuthorName = "Author Two", CategoryName = "Category Two" };

            await _bookService.AddBookAsync(book1);
            await _bookService.AddBookAsync(book2);

            var result = await _controller.GetBooks();
            var okResult = result.Result as OkObjectResult;

            Assert.NotNull(okResult);
            var books = okResult?.Value as IEnumerable<BookDto>;
            Assert.NotNull(books);
            Assert.Equal(2, books?.Count());
        }

        [Fact]
        public async Task GetBook_ReturnsCorrectBook_WhenBookExists()
        {
            var bookDto = new BookDto { Id = 1, Title = "Specific Book", Year = 2021, AuthorName = "Test Author", CategoryName = "Test Category" };
            await _bookService.AddBookAsync(bookDto);

            var result = await _controller.GetBook(1);
            var okResult = result.Result as OkObjectResult;

            Assert.NotNull(okResult);
            var returnedBook = okResult?.Value as BookDto;
            Assert.NotNull(returnedBook);
            Assert.Equal("Specific Book", returnedBook?.Title);
            Assert.Equal("Test Author", returnedBook?.AuthorName);
            Assert.Equal(2021, returnedBook?.Year);
            Assert.Equal("Test Category", returnedBook?.CategoryName);
        }

        [Fact]
        // Test to verify that PostBook returns BadRequest when the author name is invalid
        public async Task PostBook_ReturnsBadRequest_WhenAuthorNameIsInvalid()
        {
            // Arrange
            var bookDto = new BookDto
            {
                Title = "Book Without Author",
                Year = 2021,
                AuthorName = "", // Invalid author name
                CategoryName = "Test Category"
            };

            // Act
            var result = await _controller.PostBook(bookDto);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult!.StatusCode);
        }

        [Fact]
        // Test to verify that GetBooks returns books with the correct category information
        public async Task GetBooks_ReturnsBooks_WithCorrectCategoryInformation()
        {
            // Arrange
            var bookDto = new BookDto
            {
                Id = 1,
                Title = "Categorized Book",
                Year = 2023,
                AuthorName = "Author Test",
                CategoryName = "Science Fiction"
            };
            await _bookService.AddBookAsync(bookDto);

            // Act
            var result = await _controller.GetBooks();
            var okResult = result.Result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            var books = okResult?.Value as IEnumerable<BookDto>;
            Assert.NotNull(books);
            Assert.Single(books);
            var book = books?.FirstOrDefault();
            Assert.NotNull(book);
            Assert.Equal("Science Fiction", book?.CategoryName);
        }

        [Fact]
        // Test to verify that GetBookByDetails returns the correct book when author ID and title match but year does not
        public async Task GetBookByDetails_ReturnsNotFound_WhenYearDoesNotMatch()
        {
            // Arrange
            var bookDto = new BookDto
            {
                Id = 1,
                Title = "Time Travel Book",
                Year = 2025,
                AuthorName = "Time Author",
                CategoryName = "Science Fiction"
            };
            await _bookService.AddBookAsync(bookDto);

            // Act
            var result = await _controller.GetBookByDetails("Time Travel Book", 1, 2024); // Year mismatch
            var notFoundResult = result.Result as NotFoundResult;

            // Assert
            Assert.NotNull(notFoundResult);
        }

        [Fact]
        // Test to verify that PostBook returns Conflict when adding a duplicate book with the same title, author, and year
        public async Task PostBook_ReturnsConflict_WhenDuplicateBookAdded()
        {
            // Arrange
            var bookDto = new BookDto
            {
                Id = 1,
                Title = "Duplicate Book",
                Year = 2022,
                AuthorName = "Author One",
                CategoryName = "Fiction"
            };
            await _bookService.AddBookAsync(bookDto);

            // Act
            var duplicateBookDto = new BookDto
            {
                Id = 2,
                Title = "Duplicate Book",
                Year = 2022,
                AuthorName = "Author One",
                CategoryName = "Fiction"
            };
            var result = await _controller.PostBook(duplicateBookDto);

            // Assert
            var conflictResult = result.Result as ConflictObjectResult;
            Assert.NotNull(conflictResult);
            Assert.Equal(409, conflictResult!.StatusCode);
        }

        [Fact]
        public async Task PostBook_CreatesNewCategory_WhenCategoryDoesNotExist()
        {
            // Arrange: Set up a new book DTO with a category name that does not currently exist
            var newBookDto = new BookDto
            {
                Title = "New Category Book",
                Year = 2023,
                AuthorName = "Author Name",
                CategoryName = "New Unique Category" // A category that doesn't already exist
            };

            // Act: Call the controller's PostBook method with the new book DTO
            var result = await _controller.PostBook(newBookDto);

            // Assert: Verify that a new category is created
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.NotNull(createdResult); // Ensure the result is not null and of the expected type
            var createdBook = createdResult?.Value as Book;
            Assert.NotNull(createdBook); // Ensure the created book is not null
            Assert.Equal("New Unique Category", createdBook?.Category?.Name); // Check if the category name matches the new unique category
        }

        [Fact]
        public async Task PutBook_UpdatesBook_WhenAuthorIsChanged()
        {
            // Arrange: Set up a book DTO with a new author name
            var bookDto = new BookDto
            {
                Id = 1,
                Title = "Book With New Author",
                Year = 2022,
                AuthorName = "New Author Name", // New author name
                CategoryName = "Fiction"
            };

            // Add an initial book with the same ID but a different (old) author name
            await _bookService.AddBookAsync(new BookDto
            {
                Id = 1,
                Title = "Book With New Author",
                Year = 2022,
                AuthorName = "Old Author Name", // Old author name
                CategoryName = "Fiction"
            });

            // Act: Call the controller's PutBook method to update the author of the book
            var result = await _controller.PutBook(1, bookDto);

            // Assert: Verify that the update was successful and returned a NoContent (204) status code
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult); // Ensure the result is not null and of the expected type
            Assert.Equal(204, noContentResult!.StatusCode); // Verify that the status code is 204 (No Content)
        }


    }
}
