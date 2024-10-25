
using LibraryApi.Controllers;
using LibraryApi.Services;
using LibraryApi.Models;
using LibraryApi.Dtos;
using Microsoft.AspNetCore.Mvc;


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
        // Test to verify that GetBooks returns a list of books with status OK
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
        // Test to verify that GetBook returns NotFound when a book with the specified ID does not exist
        public async Task GetBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var result = await _controller.GetBook(999);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        // Test to verify that PostBook adds a new book successfully
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
        // Test to verify that DeleteBook returns NotFound when attempting to delete a non-existent book
        public async Task DeleteBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var result = await _controller.DeleteBook(999);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        // Test to verify that PutBook returns BadRequest when the IDs do not match
        public async Task PutBook_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            var bookDto = new BookDto { Id = 2, Title = "Updated Title" };
            var result = await _controller.PutBook(1, bookDto);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        // Test to verify that GetBooks returns an empty list when no books exist
        public async Task GetBooks_ReturnsEmptyList_WhenNoBooksExist()
        {
            // Act
            var result = await _controller.GetBooks();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);

            var books = okResult?.Value as IEnumerable<BookDto>;
            Assert.NotNull(books);
            Assert.Empty(books);
        }

        [Fact]
        // Test to verify that DeleteBook returns NoContent when deleting an existing book
        public async Task DeleteBook_ReturnsNoContent_WhenBookExists()
        {
            // Arrange
            var bookDto = new BookDto { Id = 1, Title = "Existing Book", AuthorName = "Test Author", Year = 2022 };
            var book = new Book { Id = 1, Title = "Existing Book", AuthorId = 1, Year = 2022 };
            await _bookService.AddBookAsync(bookDto);

            // Act
            var result = await _controller.DeleteBook(1);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult!.StatusCode);
        }

        [Fact]
        // Test to verify that PutBook returns NotFound when updating a non-existent book
        public async Task UpdateBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var bookDto = new BookDto { Id = 999, Title = "Nonexistent Book" };
            var result = await _controller.PutBook(999, bookDto);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        // Test to verify that GetBookByDetails returns NotFound when the details do not match any book
        public async Task GetBookByDetails_ReturnsNotFound_WhenDetailsDoNotMatch()
        {
            var result = await _controller.GetBookByDetails("Nonexistent Title", 999, 1999);
            var notFoundResult = result.Result as NotFoundResult;
            Assert.NotNull(notFoundResult);
        }

        [Fact]
        // Test to verify that PostBook returns BadRequest when invalid data is provided
        public async Task PostBook_ReturnsBadRequest_WhenInvalidData()
        {
            var invalidBookDto = new BookDto { Title = "", Year = -1, AuthorName = "", CategoryName = "" };
            var result = await _controller.PostBook(invalidBookDto);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult!.StatusCode);
        }

        [Fact]
        // Test to verify that PutBook returns NoContent when a book is updated successfully
        public async Task PutBook_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var bookDto = new BookDto { Id = 1, Title = "Updated Book", Year = 2022, AuthorName = "Test Author", CategoryName = "Test Category" };
            await _bookService.AddBookAsync(bookDto);

            // Act
            var result = await _controller.PutBook(1, bookDto);

            // Assert
            if (result is NoContentResult noContentResult)
            {
                Assert.Equal(204, noContentResult.StatusCode);
            }
            else
            {
                Assert.True(false, "Expected NoContentResult but got a different result.");
            }
        }

        [Fact]
        // Test to verify that PostBook returns Conflict when a book with the same details already exists
        public async Task PostBook_ReturnsConflict_WhenBookAlreadyExists()
        {
            // Arrange
            var existingBookDto = new BookDto { Id = 1, Title = "Existing Book", Year = 2022, AuthorName = "Test Author", CategoryName = "Test Category" };
            await _bookService.AddBookAsync(existingBookDto);

            var newBookDto = new BookDto { Id = 2, Title = "Existing Book", Year = 2022, AuthorName = "Test Author", CategoryName = "Test Category" };

            // Act
            var result = await _controller.PostBook(newBookDto);

            // Assert
            var conflictResult = result.Result as ConflictObjectResult;
            Assert.NotNull(conflictResult);
            Assert.Equal(409, conflictResult!.StatusCode);
        }

        [Fact]
        // Test to verify that GetBookByDetails returns the correct book when details match
        public async Task GetBookByDetails_ReturnsCorrectBook_WhenDetailsMatch()
        {
            // Arrange
            var bookDto = new BookDto { Id = 1, Title = "Unique Title", Year = 2023, AuthorName = "Specific Author", CategoryName = "Fiction" };
            await _bookService.AddBookAsync(bookDto);

            // Act
            var result = await _controller.GetBookByDetails("Unique Title", 1, 2023);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);

            var foundBook = okResult?.Value as Book;
            Assert.NotNull(foundBook);
            Assert.Equal("Unique Title", foundBook?.Title);
        }

        [Fact]
        // Test to verify that GetBooks returns the correct number of books when multiple books exist
        public async Task GetBooks_ReturnsCorrectNumberOfBooks_WhenMultipleBooksExist()
        {
            // Arrange
            var book1 = new BookDto { Id = 1, Title = "First Book", Year = 2021, AuthorName = "Author One", CategoryName = "Category One" };
            var book2 = new BookDto { Id = 2, Title = "Second Book", Year = 2022, AuthorName = "Author Two", CategoryName = "Category Two" };

            await _bookService.AddBookAsync(book1);
            await _bookService.AddBookAsync(book2);

            // Act
            var result = await _controller.GetBooks();
            var okResult = result.Result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            var books = okResult?.Value as IEnumerable<BookDto>;
            Assert.NotNull(books);
            Assert.Equal(2, books?.Count());
        }

        [Fact]
        // Test to verify that GetBook returns the correct book when it exists
        public async Task GetBook_ReturnsCorrectBook_WhenBookExists()
        {
            // Arrange
            var bookDto = new BookDto { Id = 1, Title = "Specific Book", Year = 2021, AuthorName = "Test Author", CategoryName = "Test Category" };
            await _bookService.AddBookAsync(bookDto);

            // Act
            var result = await _controller.GetBook(1);
            var okResult = result.Result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            var returnedBook = okResult?.Value as BookDto;
            Assert.NotNull(returnedBook);
            Assert.Equal("Specific Book", returnedBook?.Title);
            Assert.Equal("Test Author", returnedBook?.AuthorName);
            Assert.Equal(2021, returnedBook?.Year);
            Assert.Equal("Test Category", returnedBook?.CategoryName);
        }

        [Fact]
        // Test to verify that PostBook allows duplicate titles and years with different authors
        public async Task PostBook_AllowsDuplicateTitleAndYear_WithDifferentAuthors()
        {
            // Arrange
            var firstBookDto = new BookDto
            {
                Id = 1,
                Title = "Duplicate Title",
                Year = 2023,
                AuthorName = "First Author",
                CategoryName = "Science"
            };

            var secondBookDto = new BookDto
            {
                Id = 2,
                Title = "Duplicate Title",
                Year = 2023,
                AuthorName = "Second Author",
                CategoryName = "Science"
            };

            // Act
            var firstResult = await _controller.PostBook(firstBookDto);
            var secondResult = await _controller.PostBook(secondBookDto);

            // Assert
            var firstCreatedResult = firstResult.Result as CreatedAtActionResult;
            var secondCreatedResult = secondResult.Result as CreatedAtActionResult;

            Assert.NotNull(firstCreatedResult);
            Assert.NotNull(secondCreatedResult);
            Assert.Equal(201, firstCreatedResult!.StatusCode);
            Assert.Equal(201, secondCreatedResult!.StatusCode);
        }

        [Fact]
        // Test to verify that PostBook returns CreatedResult when a valid book is provided
        public async Task PostBook_ReturnsCreatedResult_WhenBookIsValid()
        {
            // Arrange
            var validBookDto = new BookDto
            {
                Title = "Simple Book",
                Year = 2023,
                AuthorName = "Simple Author",
                CategoryName = "Simple Category"
            };

            // Act
            var result = await _controller.PostBook(validBookDto);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult!.StatusCode);
        }

        [Fact]
        // Test to verify that PutBook returns NoContent when a book is updated successfully
        public async Task PutBook_ReturnsNoContent_WhenBookIsUpdatedSuccessfully()
        {
            // Arrange
            var bookDto = new BookDto
            {
                Id = 1,
                Title = "Updated Title",
                Year = 2023,
                AuthorName = "Updated Author",
                CategoryName = "Updated Category"
            };

            await _bookService.AddBookAsync(bookDto);

            // Act
            var result = await _controller.PutBook(1, bookDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        // Test to verify that PostBook returns Created when a valid book is provided
        public async Task PostBook_ReturnsCreated_WhenValidBookIsProvided()
        {
            // Arrange
            var newBookDto = new BookDto
            {
                Title = "Valid Book",
                Year = 2023,
                AuthorName = "Valid Author",
                CategoryName = "Valid Category"
            };

            // Act
            var result = await _controller.PostBook(newBookDto);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult!.StatusCode);
        }

    }
}
