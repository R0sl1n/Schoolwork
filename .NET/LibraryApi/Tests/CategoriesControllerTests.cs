using LibraryApi.Controllers;
using LibraryApi.Services;
using LibraryApi.Models;
using LibraryApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class CategoriesControllerTests
    {
        private readonly CategoryService _categoryService;
        private readonly CategoriesController _controller;

        // Constructor to set up the controller and service for testing
        public CategoriesControllerTests()
        {
            _categoryService = MockHelpers.MockCategoryService();
            _controller = new CategoriesController(_categoryService);
        }

        [Fact]
        // Test to verify that GetCategories returns a list of categories with status OK
        public async Task GetCategories_ReturnsOkResult_WithListOfCategories()
        {
            var result = await _controller.GetCategories();
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            var categories = okResult?.Value as IEnumerable<CategoryDto>;
            Assert.NotNull(categories);
        }

        [Fact]
        // Test to verify that GetCategory returns NotFound when a category with the specified ID does not exist
        public async Task GetCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            var result = await _controller.GetCategory(999);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        // Test to verify that PostCategory adds a new category successfully
        public async Task PostCategory_AddsCategorySuccessfully()
        {
            var newCategory = new Category { Id = 3, Name = "Science" };
            var result = await _controller.PostCategory(newCategory);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.NotNull(createdResult);
            var createdCategory = createdResult?.Value as Category;
            Assert.NotNull(createdCategory);
            Assert.Equal(newCategory.Name, createdCategory?.Name);
        }

        [Fact]
        // Test to verify that DeleteCategory returns NotFound when attempting to delete a non-existent category
        public async Task DeleteCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            var result = await _controller.DeleteCategory(999);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        // Test to verify that GetCategoryByName returns OK and the correct category when the category exists
        public async Task GetCategoryByName_ReturnsOkResult_WhenCategoryExists()
        {
            // Arrange
            var categoryName = "Science";
            await _categoryService.AddCategoryAsync(new Category { Id = 1, Name = categoryName });

            // Act
            var result = await _controller.GetCategoryByName(categoryName);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            var category = okResult?.Value as CategoryDto;
            Assert.NotNull(category);
            Assert.Equal(categoryName, category?.Name);
        }

        [Fact]
        // Test to verify that PutCategory returns NotFound when attempting to update a non-existent category
        public async Task PutCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var category = new Category { Id = 999, Name = "Non-Existent Category" };

            // Act
            var result = await _controller.PutCategory(999, category);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        // Test to verify that PutCategory returns BadRequest when the category ID does not match the route ID
        public async Task PutCategory_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var category = new Category { Id = 3, Name = "Fiction" };

            // Act
            var result = await _controller.PutCategory(1, category);

            // Assert
            var badRequestResult = result as BadRequestResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult!.StatusCode);
        }

        [Fact]
        // Test to verify that GetCategoryByName returns NotFound when the specified category name does not exist
        public async Task GetCategoryByName_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            var result = await _controller.GetCategoryByName("Unknown Category");
            var notFoundResult = result.Result as NotFoundResult;
            Assert.NotNull(notFoundResult);
        }

        [Fact]
        // Test to verify that PostCategory returns Conflict when a category with the same name already exists
        public async Task PostCategory_ReturnsConflict_WhenCategoryAlreadyExists()
        {
            // Arrange
            var category = new Category { Name = "Duplicate Category" };
            await _categoryService.AddCategoryAsync(category);

            // Act
            var duplicateCategory = new Category { Name = "Duplicate Category" };
            var result = await _controller.PostCategory(duplicateCategory);

            // Assert
            var conflictResult = result.Result as ConflictObjectResult;
            Assert.NotNull(conflictResult);
            Assert.Equal(409, conflictResult!.StatusCode);
        }

        [Fact]
        // Test to verify that PostCategory returns BadRequest when the model state is invalid
        public async Task PostCategory_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var category = new Category(); // Missing required fields
            var result = await _controller.PostCategory(category);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult!.StatusCode);
        }

        [Fact]
        // Test to verify that DeleteCategory returns NoContent when the category is successfully deleted
        public async Task DeleteCategory_ReturnsNoContent_WhenCategoryIsDeleted()
        {
            // Arrange
            var category = new Category { Name = "Temporary Category" };
            var addedCategory = await _categoryService.AddCategoryAsync(category);

            // Act
            var result = await _controller.DeleteCategory(addedCategory.Id);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult!.StatusCode);
        }
    }
}
