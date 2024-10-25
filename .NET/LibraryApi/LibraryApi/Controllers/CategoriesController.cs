using Microsoft.AspNetCore.Mvc;
using LibraryApi.Services;
using LibraryApi.Dtos;
using LibraryApi.Models;

namespace LibraryApi.Controllers
{
    // API Controller for managing categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        // Constructor to initialize the CategoryService dependency
        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/categories
        // Returns a list of all categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories); // Returns 200 OK with the list of categories
        }

        // GET: api/categories/{id}
        // Returns a specific category by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound(); // Returns 404 if the category is not found
            return Ok(category); // Returns 200 OK with the category details
        }

        // GET: api/categories/ByName?name={name}
        // Returns a category by its name
        [HttpGet("ByName")]
        public async Task<ActionResult<CategoryDto?>> GetCategoryByName([FromQuery] string name)
        {
            var category = await _categoryService.GetCategoryByNameAsync(name);
            if (category == null) return NotFound(); // Returns 404 if no category matches the name
            // Returns 200 OK with category details and the list of book titles under this category
            return Ok(new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                BookTitles = category.Books?.Select(b => b.Title ?? string.Empty).ToList() ?? new List<string>()
            });
        }

        // POST: api/categories
        // Adds a new category
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostCategory(Category category)
        {
            var createdCategory = await _categoryService.AddCategoryAsync(category);
            // Returns 201 Created with the location of the newly created category
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
        }

        // PUT: api/categories/{id}
        // Updates an existing category by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
                return BadRequest(); // Returns 400 Bad Request if the provided ID does not match the category's ID

            var success = await _categoryService.UpdateCategoryAsync(category);
            if (!success)
                return NotFound(); // Returns 404 Not Found if the category does not exist

            // Returns 204 No Content if the update is successful
            return NoContent();
        }

        // DELETE: api/categories/{id}
        // Deletes a category by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var success = await _categoryService.DeleteCategoryAsync(id);
            if (!success)
                return NotFound(); // Returns 404 if the category does not exist

            // Returns 204 No Content if the deletion is successful
            return NoContent();
        }
    }
}
